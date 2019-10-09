# Kifreak.MultiThread
Library to externalize the Threads.

**Async Methods (new part)**

1) Create a class inherit from IMultipleModel. Have 4 methods:
    - ActionToExecute (CancellationToken): Action to execute in the Thread.
    - Progress: Defined how you want to defined the progress in your own action. The global Progress will be calculated in based of this progress.
    - IsCompleted: Indicates when the action is completed.
    - IsCanceled: Indicates when the action is canceled (the program take 2 parameter to know when an action is canceled: This method or Task.IsCanceled)
2) Instantiate MutipleBase class (you can create your own Multiple class using IMultiple interface):
    - Run(IEnumerable<IMultipleModel>): Create the task and run all threads.
    - Wait(): Async method to wait until all task are completed or canceled.
    - Progress(): Get Global progress.
    - CancelTask(ThreadModel): Cancel an specific thread.
    - Dispose(): Cancel all tasks.
