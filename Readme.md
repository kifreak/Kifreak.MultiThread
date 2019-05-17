# Kifreak.MultiThread

**[Obsolete parte]**
*Notes: Doesn't work with Async Methods.*
1) Instantiate an array of MultiModel with:
		- Functions to execute
		- Parameter to Send to the action (List, string ,etc).
2) Instantiate MulTithreads with Static Init Method. And Call RunAsync.
* See UnitTest for an code Example.

**Async Methods (new part)**
1) Create a class inherit from IMultipleModel. Have 4 methods:
    - ActionToExecute (CancellationToken): Action to execute in the Thread.
    - Progress: Defined how you want to defined the progress in your own action. The global Progress will be calculated in based of this progress.
    - IsCompleted: Indicates when the action is completed.
    - IsCanceled: Indicates when the action is canceled (the program take 2 parameter to know when an action is canceled: This method or Task.IsCanceled)
2) Instantiate Mutiple class (you can create your own Multiple class using IMultiple interface):
    - Run(IEnumerable<IMultipleModel>): Create the task and run all threads.
    - Wait(): Async method to wait until all task are completed or canceled.
    - Progress(): Get Global progress.
    - CancelTask(ThreadModel): Cancel an specific thread.
    - Dispose(): Cancel all tasks.
