//
// Copyright 2004-2005 Jim Wiese  All Rights Reserved
//
// This code is covered by the Creative Commons license (see license.html) or
// http://creativecommons.org/licenses/by-nc-sa/2.0/legalcode or
// http://creativecommons.org/licenses/by-nc-sa/2.0/ for more information.
// 
// I'm pretty liberal in giving out free commercial licenses, so please contact 
// me if you would like to use this code in a commercial 
// project: jcwiese@yahoo.com . 
//
using System;
using System.Collections ;
using System.Threading ;
using System.Diagnostics ;

namespace CodeProject.Threading
{
	/// <summary>
	/// Exception defined primarily for use with the FeedingStack.  When thrown, this 
	/// exception indicates that there are no more elements that will be comming from
	/// any other threads.
	/// </summary>
	public class NoMoreException			: ApplicationException  
	{
		public NoMoreException() : base() {}
		public NoMoreException( string s ) : base(s) {}
		public NoMoreException( string s, Exception exp ) : base(s,exp) {} 
	}

	/// <summary>
	/// Exception defined to determine that there are no more producers for a stack
	/// </summary>
	public class NoMoreProducersException	: NoMoreException 
	{
		public NoMoreProducersException() : base() {}
		public NoMoreProducersException( string s ) : base(s) {}
		public NoMoreProducersException( string s, Exception exp ) : base(s,exp) {} 
	}
	
	
	public interface IProducer 
	{
		void		AddProducer() ;
		void		AddProducer( Thread toProduce ) ;
		void		RemoveProducer( Thread notProducing ) ;
		void		RemoveProducer() ;
	}
	
	public class ThreadQueue : IProducer, IDisposable
	{
		Queue				m_queue				= null ;
		object				SyncRoot			= new object() ;
		int					m_lowWater			= 5 ;
		int					m_highWater			= 50 ;
		object				m_toAddWait			= new object() ;
		object				m_toRemoveWait		= new object() ;
		bool				m_enableHigh		= true ;
		bool				m_waitForLowWater	= false ;
		protected int		m_producersAvail	= 0 ;
		protected Hashtable	m_producers			= Hashtable.Synchronized( new Hashtable() ) ;
		
		readonly static TimeSpan INFINITE		= new TimeSpan( Timeout.Infinite ) ;
		
		public bool			EnableHighWater 
		{
			set { m_enableHigh = value ; }
			get { return m_enableHigh ; }
		}

		public				ThreadQueue() 
		{
			m_queue	= Queue.Synchronized( new Queue() ) ;
		}
		public				ThreadQueue( ICollection col )  
		{
			m_queue = Queue.Synchronized( new Queue( col ) ) ;
		}

		public				ThreadQueue( int size ) 
		{
			m_queue	= Queue.Synchronized( new Queue( size ) ) ;
		}
		/// <summary>
		/// Clear all of the items from the queue
		/// </summary>
		public void			Clear() 
		{
			lock( m_toRemoveWait ) 
			{
				m_queue.Clear() ;
			}

			lock( m_toAddWait ) 
			{
				// Indicate that items may now be added to this queue
				m_waitForLowWater	= false ;

				Monitor.Pulse( m_toAddWait ) ;
			}
		}
		public int			Count 
		{
			get { return m_queue.Count ; } 
		}
		public int			LowWater 
		{
			set 
			{
				lock( SyncRoot )
					m_lowWater	= value ;
			}
			get 
			{
				return m_lowWater ;
			}
		}
		public int			HighWater 
		{
			set 
			{
				lock( SyncRoot )
					m_highWater	= value ;
			}
			get 
			{
				return m_highWater ;
			}
		}

		public int			ProducersAvailable 
		{
			get { return m_producersAvail ; }
		}
		/// <summary>
		/// Add the specified item to the stack, but only wait the specified amount
		/// of time for the queue to become available
		/// </summary>
		/// <param name="toAdd">Item to add to the queue</param>
		/// <param name="timeToWait">Time to wait for the queue to become available</param>
		/// <returns>True if the item was successfully added to the queue</returns>
		public bool			Add( object toAdd, TimeSpan timeToWait ) 
		{
			bool		success		= false ;
			DateTime	expiration	= 
				timeToWait == INFINITE ? DateTime.MaxValue : DateTime.Now  + timeToWait ;

			lock( m_toAddWait ) 
			{
				// Determine if we should wait at all
				if ( m_waitForLowWater || (EnableHighWater && Count >= HighWater) ) 
				{
					// Indicate that we've hit the high water mark, and should wait for the low water
					m_waitForLowWater		= true ;

					while(	!success					&& 
							(DateTime.Now < expiration) &&
							Count >= LowWater			) 
					{
						DateTime	now			= DateTime.Now ;
						TimeSpan	remaining	= timeToWait == INFINITE ? INFINITE : expiration - now ;

						success		= false ;

						// Make sure we don't wait too long
						if ( remaining.Ticks > 0 )
							success	= Monitor.Wait( m_toAddWait, remaining ) ;
						else if ( remaining == INFINITE  )
							success	= Monitor.Wait( m_toAddWait ) ;
					}
				}
				else
					// Simply add this item because we don't have to obey the high water mark
					success			= true ;
				
				if ( success ) 
				{ 
					m_queue.Enqueue( toAdd ) ;
				}
			}

			if ( success )
				lock( m_toRemoveWait )
					Monitor.Pulse( m_toRemoveWait ) ;

			return success ;
		}
		/// <summary>
		/// Add the item to the queue
		/// </summary>
		/// <param name="toAdd">Item to add to the queue</param>
		/// <param name="shouldWait">True if the thread should block until
		/// the queue is is available</param>
		public void			Add( object toAdd, bool shouldWait ) 
		{
			lock( m_toAddWait ) 
			{
				// Check to see if we've hit the high water mark
				if ( EnableHighWater && Count >= HighWater )
					m_waitForLowWater	= true ;

				if ( shouldWait )
					while ( m_waitForLowWater || (EnableHighWater && Count >= HighWater) ) 
					{
						Monitor.Wait( m_toAddWait ) ;
					}
				
				m_queue.Enqueue( toAdd ) ;
			}

			lock( m_toRemoveWait )
				Monitor.Pulse( m_toRemoveWait ) ;
		}	
		public void			Add( object toAdd ) 
		{
			Add( toAdd, true ) ;
		}
		public object		Remove( TimeSpan toWait ) 
		{
			object	toReturn	= null ;

			lock( m_toRemoveWait ) 
			{
				DateTime	endTime		= DateTime.Now + toWait  ;
				TimeSpan	timeLeft	= endTime - DateTime.Now ;

				bool	hasLock				= true ;
				bool	noMoreProducers		= false ;
				bool	shouldAddProducer	= false ;

				// If we are a producer for our own stack, remove it from the available list
				if ( m_producers.ContainsKey( Thread.CurrentThread ) || 
					 m_producers.ContainsKey( Thread.CurrentThread.Name ) ) 
				{
					m_producersAvail	-=	1 ;
					shouldAddProducer	= true ;
				}

				try 
				{
					while( Count == 0 && (timeLeft.Ticks > 0 || toWait == INFINITE) ) 
					{
						// Determine if there are any more producers
						if ( m_producersAvail <= 0 ) 
						{
							// Indicate that there are no more prodcuers for this thread
							noMoreProducers		= true ;

							// Don't add this back as a producer, otherwise, if we signal
							// the next thread, then it will think that this thread is
							// still producing
							shouldAddProducer	= false ;

							break ;
						}

						if ( toWait == INFINITE )
							hasLock	= Monitor.Wait( m_toRemoveWait ) ;
						else
							hasLock	= Monitor.Wait( m_toRemoveWait , timeLeft ) ;

						timeLeft	= endTime - DateTime.Now ;
					}
				}
				finally 
				{
					// If we are a producer for our own stack, add it from the available list
					if ( shouldAddProducer ) 
						m_producersAvail	+= 1 ;
				}

				// Get the item off the stack, making sure that we have the lock 
				// which we wouldn't get if the time expired in 
				// Monitor.Wait( lock, timeLeft )
				if ( hasLock && Count > 0  )
					toReturn	= m_queue.Dequeue() ;
				else if ( Count == 0 && noMoreProducers )
				{
					// If this isn't going to be removed (which will cause a pulse),
					// alert the next guy so it can wake up and check
					Monitor.Pulse( m_toRemoveWait ) ;

					// If there were no more producers, then throw the exception
					throw new NoMoreProducersException("No more producers are available") ;
				}
			}
			
			if ( toReturn != null && Count <= LowWater )
				lock( m_toAddWait ) 
				{
					// Indicate that items may now be added to this queue
					m_waitForLowWater	= false ;

					Monitor.Pulse( m_toAddWait ) ;
				}
			
			return toReturn ;
		}

		public object		Remove() 
		{
			return Remove( INFINITE ) ;
		}

		public ICollection	Remove( int numToRemove, TimeSpan toWait, bool atLeastOne ) 
		{
			ArrayList	toReturn	= new ArrayList( numToRemove ) ;

			if ( numToRemove <= 0 )
				return toReturn ;

			DateTime	endTime		= DateTime.Now + toWait ;
			TimeSpan	timeLeft	= endTime - DateTime.Now ;

			while(	(atLeastOne && toReturn.Count == 0)					||	// We are required to return at least one, or...
					((toWait == INFINITE || timeLeft.Ticks > 0)
						&& toReturn.Count < numToRemove ) )					// enough items have been pulled off
			{
				// If at least one is required, reset the time left to wait
				if ( toReturn.Count == 0 && atLeastOne )
					timeLeft	= toWait ;

				try 
				{
					object	removed	= Remove( timeLeft ) ;

					if ( removed != null )
						toReturn.Add( removed ) ;
				}
				catch( NoMoreException exp ) 
				{

					// Notify any others waiting
					lock( m_toRemoveWait )
						Monitor.Pulse( m_toRemoveWait ) ;

					// If we had already collected some items, return them, otherwise
					// simply rethrow the exception
					if ( toReturn.Count > 0 ) 
					{
						break ;					// Some elements were retrieved
					}
					else
						throw exp ;				// No more elements were retrieved
				}

				timeLeft	= endTime - DateTime.Now ;
			}
			

			return toReturn ;
		}

		
		public void			AddProducerByName( string producer )
		{ 
			lock( m_toRemoveWait ) 
			{
				if ( !m_producers.ContainsKey( producer ) ) 
					m_producersAvail += 1 ;

				m_producers[ producer ]		= producer ;
			}
		}
		public void			AddProducer( Thread producer ) 
		{
			lock( m_toRemoveWait ) 
			{
				if ( !m_producers.ContainsKey( producer ) ) 
				{
					m_producersAvail += 1 ;
					m_producers[ producer ]		= producer ;
				}
			}
		}

		public void			RemoveProducer( Thread producer ) 
		{
			lock( m_toRemoveWait ) 
			{
				if ( m_producers.ContainsKey( producer ) ) 
				{
					m_producers.Remove( producer ) ;
					m_producersAvail	-= 1 ;
				}
				else 
				{
					Trace.WriteLine( 
						string.Format(
							"Attempting to remove producer that wasn't a producer for this ThreadQueue: {0}", 
							producer.Name ),
						"Error" ) ;
				}

				// Notify any consumers
				Monitor.Pulse( m_toRemoveWait ) ;
			}
		}
		public void			RemoveProducerByName( string producer ) 
		{
			lock( m_toRemoveWait ) 
			{
				if ( m_producers.ContainsKey( producer ) ) 
				{
					m_producers.Remove( producer ) ;
					m_producersAvail	-= 1 ;
				}
				else 
				{
					Trace.WriteLine( 
						string.Format(
							"Attempting to remove producer that wasn't a producer for this ThreadQueue: {0}", 
							producer ),
						"Error" ) ;
				}

				// Notify any consumers
				Monitor.Pulse( m_toRemoveWait ) ;
			}
		}
		public void			RemoveAllProducers() 
		{
			lock( m_toRemoveWait ) 
			{
				m_producers.Clear() ;
				m_producersAvail	= 0 ;

				// Notify all the consumers
				Monitor.PulseAll( m_toRemoveWait ) ;
			}
		}

		public void			AddProducer() 
		{
			AddProducer( Thread.CurrentThread ) ;
		}
		public void			RemoveProducer() 
		{
			RemoveProducer( Thread.CurrentThread ) ;
		}
	
		#region Unit Test 
//		public static void Main( string[] args ) 
//		{
//
//			if ( args != null && args.Length > 0 ) 
//			{
//				foreach( string arg in args ) 
//				{
//					switch( arg ) 
//					{
//
//						case "-unittest":
//							//
//							// Unit test
//							//
//							Environment.Exit( ThreadQueue.UnitTest() ) ;
//							break ;
//
//						default:
//							Console.WriteLine("Error: unknown argument {0}", arg ) ;
//							break ;
//					}
//				}
//			}
//		}

		public static int UnitTest() 
		{
			
			// Generic item to produce random values
			Random			rand	= new Random() ;

			string	testDescription		=
				string.Format(
					"***\n"																			+
					"*** Unit test to ensure that an item only comes off of the queue once, not\n"	+
					"*** multiple times for each Remove() operation\n"								+
					"***\n" ) ;

			Console.WriteLine( testDescription ) ;
 
			ArrayList	allThreads	= new ArrayList() ;
			ArrayList	consumers	= new ArrayList() ;
			ThreadQueue	startQueue	= new ThreadQueue() ;
			ThreadQueue	feedQueue	= new ThreadQueue() ;

			feedQueue.HighWater		= rand.Next( 11, 250 ); 
			feedQueue.LowWater		= rand.Next( 1, feedQueue.HighWater ) ;
			feedQueue.EnableHighWater	= true ;

			// Create a queue, and start running against it
			for( int i = 0 ; i < rand.Next( 7, 23 ) ; i++ ) 
			{ 
				UnitTestThread	feeder	= 
					new UnitTestThread( startQueue, feedQueue, new TimeSpan( Timeout.Infinite ), rand.Next(1, 100)  ) ;

				Thread	feederThread	= new Thread( new ThreadStart( feeder.Start ) ) ;
				feederThread.Name		= string.Format( "Feeder {0}", i ) ;

				feedQueue.AddProducer( feederThread ) ;

				allThreads.Add( feederThread ) ;
			}

			for( int i = 0 ; i < rand.Next( 2, 7 ) ; i++ ) 
			{ 
				// Note:, the first thread will always do one item at a time 
				int				numItems	= i == 0 ? 
					1 : 
					rand.Next( 2, 23 ) ;
				
				TimeSpan		toWait		= i == 0 ? 
					new TimeSpan( Timeout.Infinite ) :
					new TimeSpan(  0, 0, 0, 0, rand.Next( 10, 10 * 1000 ) ) ;

				UnitTestThread	consumer	= 
					new UnitTestThread( feedQueue, null, toWait, numItems ) ;

				Thread	consumerThread	= new Thread( new ThreadStart( consumer.Start ) ) ;
				consumerThread.Name		= 
					string.Format( "Consumer: {0}, numPerCall: {1}, toWait: {2}", i, numItems, toWait ) ;

				consumers.Add( consumer ) ;
				allThreads.Add( consumerThread ) ;
			}


			// Queue up some items 
			int		totalItems		= rand.Next( 10 * 1000, 20 * 1000 ) ;
			Console.WriteLine( "Total items to process: {0}", totalItems ) ;
			for( int i = 0 ; i < totalItems ; i++ ) 
			{
				startQueue.Add( i, false ) ;
			}

			foreach( Thread thd in allThreads ) 
			{
				Console.WriteLine( "Starting thread: {0}...", thd.Name ) ;
				thd.Start() ;
			}

			// Wait for all the threads to finish 

			bool	isAlive	= false ;

			do 
			{ 
				Console.WriteLine( 
					"StartQueue: {0} items, FeedQueue: {1} (LowWater: {2}, HighWater: {3})", 
					startQueue.Count, 
					feedQueue.Count,
					feedQueue.LowWater,
					feedQueue.HighWater ) ;

				isAlive		= false ;

				foreach( Thread thd in allThreads ) 
				{
					isAlive		|= thd.IsAlive ;

					if ( isAlive ) 
						break ;
				}

				if ( isAlive ) 
				{
					Thread.Sleep( 2 * 1000 ) ;
				}
			}
			while( isAlive ) ;

			Console.WriteLine( 
				"*Finished: StartQueue: {0} items, FeedQueue: {1}", startQueue.Count, feedQueue.Count ) ;

			Console.WriteLine("Consumer counts: ") ;

			long	consumerCount	= 0 ;

			foreach( UnitTestThread consumer in consumers ) 
			{
				Console.WriteLine( "\t{0} items", consumer.NumProcessed ) ;

				consumerCount	+= consumer.NumProcessed ;
			}

			if ( consumerCount != totalItems )
				Console.WriteLine(	"***\n" + 
									"*** Failed test, consumers consumed {0} of {1} items\n" +
									"***\n", 
									consumerCount, totalItems )  ;
			else
				Console.WriteLine(	"***\n" +
									"*** Passed test, consumers consumed {0} of {1} items\n" +
									"***\n", 
									consumerCount, totalItems ) ;

			//
			// Unit test for items flowing from one Queue to another to make sure all the
			// items get from the head to the tail
			//


			// Create several queues to shuffle the objects along
			ThreadQueue[]	queues	= new ThreadQueue[ rand.Next(5, 15) ] ;
			for( int i = 0 ; i < queues.Length ; i++ )
				queues[i]	= new ThreadQueue() ;

			Console.WriteLine( "* Created {0} queues", queues.Length ) ;

			// Prime the first queue with some objects
			totalItems		= rand.Next( 10, 20000 ) ;
			for( int i = 0 ; i < totalItems ; i++ )
				queues[0].Add( rand.Next( -10000, -1000 ).ToString(), false ) ;

			Console.WriteLine( "* Primed the first queue with {0} items",
				totalItems ) ;

			ArrayList	allThds	= new ArrayList() ;

			Console.Write("* Threads per queue (queue number: thread count):\n*") ;

			// Setup threads to read from the queues and write to the next queue
			for( int i = 0 ; i < queues.Length - 1; i++ ) 
			{	
				int numThds  ;

				// Create at a minimum of threads reading from each queue
				for( numThds = 0 ; numThds < rand.Next( 5, 30 ) ; numThds++ ) 
				{
					ThreadQueue		fromQueue= queues[i] ;
					ThreadQueue		toQueue = (i == queues.Length) ? null : queues[i+1] ;
					UnitTestThread	test	= new UnitTestThread( fromQueue, toQueue ) ;
					
					// Create the thread
					Thread			thd		= new Thread( new ThreadStart( test.Start ) ) ;
					thd.Name				= String.Format("{0} -> {1}", i, i+1 ) ;

					if ( i == 1 )
						// Make this thread a producer of itself 
						fromQueue.AddProducer( thd ) ;
					
					// Set the thread as a producer for the next queue
					toQueue.AddProducer( thd ) ;


					allThds.Add( thd ) ;
				}
				
//					// Add one loop that continually takes and item from it's queue and 
//					// places it back up the stream some where
//
//					int  earlier	= rand.Next( 0, i - 1 ) ;
//					readFromOwn = new UnitTestThread( queues[i], queues[i+1], queues[ earlier ] );
//					loop		= new Thread( new ThreadStart(readFromOwn.Start ) ) ;
//					loop.Name	= String.Format("{0} -> {1}", i, earlier ) ;
//					allThds.Add( loop ) ;
//
//					numThds++ ;


				Console.Write(" {0}:{1},", i , numThds ) ;
			}

			Console.WriteLine("") ;

			// Disable the high water on the last queue
			queues[ queues.Length - 1 ].EnableHighWater = false ;

			// Start all the threads
			foreach( Thread thd in allThds )
				thd.Start() ;

			DateTime	endTime	= DateTime.Now + new TimeSpan( 0, 10, 0 ) ;
 
			// Wait for all of the elements to end up in the last queue
			while(	queues[ queues.Length -1 ].Count < totalItems &&
					DateTime.Now < endTime ) 
			{
				Thread.Sleep( 5000 ) ;
				Console.WriteLine( "* {0} of {1} items have arrived in the final queue (#{2})",
					queues[ queues.Length -1 ].Count,
					totalItems,
					queues.Length - 1) ;
			}

			int	waitingThreads	= 0 ;

			// Kill all of the threads
			foreach( Thread thd in allThds ) 
			{
				thd.Join( new TimeSpan( 0, 0, 5 ) ) ;
				if ( thd.IsAlive ) 
				{
					if ( (thd.ThreadState & System.Threading.ThreadState.WaitSleepJoin) != 0 )
						waitingThreads++ ;
					thd.Abort() ;
				}
			}

			int	itemsProcessed	= queues[ queues.Length -1 ].Count ;

			if ( itemsProcessed != totalItems || waitingThreads != 0 )
				Console.WriteLine("* Failed test, only {0} of {1} items were processed " +
					" and {2} of {3} threads were waiting",
					itemsProcessed, totalItems, waitingThreads, allThds.Count ) ;
			else
				Console.WriteLine("* Passed test, final queue has {0} of {1} items, " +
					"{2} of {3} threads were waiting",
					itemsProcessed, 
					totalItems,
					waitingThreads ,
					allThds.Count ) ;

			int		toReturn	= (itemsProcessed == totalItems) ? 0 : -1 ;

			return toReturn ;
		}

		class UnitTestThread 
		{
			ThreadQueue					m_from		= null ;
			ThreadQueue					m_to		= null ;
			Random						m_rand		= new Random() ;
			int							m_numItems	= 1 ;

			readonly static TimeSpan	INFINITE	= new TimeSpan( Timeout.Infinite ) ;
			
			TimeSpan					m_toWait	= INFINITE ;

			public long					NumProcessed= 0 ;

			public		UnitTestThread( ThreadQueue readFrom, ThreadQueue writeTo ) 
			{
				m_from	= readFrom	;
				m_to	= writeTo	;
			}

			public		UnitTestThread( ThreadQueue readFrom, ThreadQueue writeTo, TimeSpan toWait, int numItems ) 
			{
				m_from		= readFrom ;
				m_to		= writeTo  ;
				m_numItems	= Math.Max( 1, numItems ) ;
				m_toWait	= toWait ;
			}

			public void Start() 
			{
				if ( m_to != null )
					m_to.AddProducer() ;

				// While there are elements available, read from one and write to 
				// the other
				try 
				{
					while ( true ) 
					{
						try 
						{

							object	workToDo	= null ;
							
							if ( m_numItems == 1 && m_toWait == INFINITE )
								workToDo	= m_from.Remove() ;
							else
								workToDo	= m_from.Remove( m_numItems, m_toWait, true ) ;

							// Random sleep time to simulate work
							Thread.Sleep( m_rand.Next( 1, 5 ) ) ;

							if ( !(workToDo is ICollection) ) 
							{
								// Increment the number processed
								Interlocked.Increment( ref NumProcessed ) ;

								// Add this to the next queue
								if ( m_to != null )
									m_to.Add( workToDo ) ;
							}
							else 
							{
//								foreach( object work in (ICollection)workToDo ) 
//								{
//									// Increment the number processed
//									Interlocked.Increment( ref NumProcessed ) ;
//								}

								NumProcessed	+= ((ICollection)workToDo).Count ;

								// Add these to the next queue
								foreach( object work in (ICollection)workToDo ) 
								{
									if ( m_to != null )
										while( !m_to.Add( work, m_toWait ) )
											Thread.Sleep( 23 ) ;

									// Random sleep time to simulate work
									Thread.Sleep( m_rand.Next( 1, 5 ) ) ;
								}
							}
						}
						catch( NoMoreException ) 
						{
							break ; // No more elements were available
						}
					}
				}
				finally 
				{
					// Remove the producer relationships

					if ( m_to != null )
						m_to.RemoveProducer() ;

					if ( m_from != null )
						m_from.RemoveProducer() ;
				}
			}
		}
		#endregion 

		#region IDisposable Members

		public void Dispose()
		{
			// Force the removal of all the producers for this stack
			RemoveAllProducers() ;
		}

		#endregion
	}

	/// <summary>
	/// Class to provide for a collection of ThreadQueues
	/// </summary>
	public class ThreadQueueCollection : System.Collections.DictionaryBase 
	{
		public ThreadQueue this[ String key ]  
		{
			get  
			{
				return( (ThreadQueue)Dictionary[key] );
			}
			set  
			{
				Dictionary[key] = value;
			}
		}

		public ICollection Keys  
		{
			get  
			{
				return( Dictionary.Keys );
			}
		}

		public ICollection Values  
		{
			get  
			{
				return( Dictionary.Values );
			}
		}

		public void Add( String key, ThreadQueue value )  
		{
			Dictionary.Add( key, value );
		}

		public bool Contains( String key )  
		{
			return( Dictionary.Contains( key ) );
		}

		public void Remove( String key )  
		{
			Dictionary.Remove( key );
		}

		protected override void OnInsert( Object key, Object value )  
		{
			if ( !(value is ThreadQueue) )
				throw new ArgumentException( "Value must be of type ThreadQueue.", "value" );

			if ( key.GetType() != Type.GetType("System.String") )
				throw new ArgumentException( "Key must be of type String.", "key" );
		}

		protected override void OnRemove( Object key, Object value )  
		{
			if ( key.GetType() != Type.GetType("System.String") )
				throw new ArgumentException( "key must be of type String.", "key" );
		}

		protected override void OnSet( Object key, Object oldValue, Object newValue )  
		{
			if ( key.GetType() != Type.GetType("System.String") )
				throw new ArgumentException( "key must be of type String.", "key" );

			if ( !(newValue is ThreadQueue) )
				throw new ArgumentException( "newValue must be of type ThreadQueue.", "newValue" );
		}

		protected override void OnValidate( Object key, Object value )  
		{
			if ( key.GetType() != Type.GetType("System.String") )
				throw new ArgumentException( "key must be of type String.", "key" );


			if ( !(value is ThreadQueue) )
				throw new ArgumentException( "value must be of type ThreadQueue.", "value" );
		}
		
		public void RemoveProducers() 
		{
			foreach( ThreadQueue queue in this.Values ) 
				queue.RemoveAllProducers() ;
		}
	}
}

