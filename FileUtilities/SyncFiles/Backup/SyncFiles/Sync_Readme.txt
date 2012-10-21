
[How Sync Settings affect Syncing...]

--------------------------------------------------------------------------------------------------
file 1					|     file 1				| same size = not copy (setting), otherwise 
(newer, same size)		|							| copy left to right (use modified date)
						|							|
file 2					|     file 2 (newer,		| same size = not copy (setting), otherwise 
						|     same size)			| copy right to left (use modified date) but only if Bidirect is on,
						|					        | copy left to right (if force copy is on) (option Conflict)
						|					        |
file 3 					|     file 3				| copy left to right (use modified date) - Always
(newer, diff size)	    |							|
						|							|
file 4					|	  file 4 (newer, 		| copy right to left (if bi-sync), if force left-to 
						|     diff size)			| right, copy left to right. (option conflict)
						|							|
						|							|
file 5					|	  x (does not exist)	| copy to right (always) - unless there is a setting?
x (does not exist)		|	  file 6				| copy to left if bi-directional sync is on.
						|							|
						
						
[Syncing with no Settings (default settings)]

--------------------------------------------------------------------------------------------------
file 1					|     file 1				| not copied 
(newer, same size)		|							| 
						|							|
file 2					|     file 2 (newer,		| not copied
						|     same size)			| 
						|					        | 
						|					        |
file 3 					|     file 3				| copy left to right (use modified date) - Always
(newer, diff size)	    |							|
						|							|
file 4					|	  file 4 (newer, 		| not copied
						|     diff size)			| 
						|							|
						|							|
file 5					|	  x (does not exist)	| copy left to right - Always
x (does not exist)		|	  file 6				| not copied
						|							|


[bidirectional sync will]
make file2 copy (right to left), file4 (right to left),file6 (right to left)

[force source sync] - will work like most standard sync then (terrible)
make file1 copy (left to right), file2 copy (left to right), file 4 (left to right);
~force source sync forces same size and date onto destination folder which is how some
 sync programs work. It most of the time results in very slow operation and is not recommended setting.

[disabling m] ~used in conjunction with bi-direct sync (forces newer files to be copied left/right)
this is how most std sync works. Copies any file that is newer regardless of size from left to right.
file1 is copied (left to right), copy file3 (left to right),...etc

