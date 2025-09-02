# Prerequisites

- .NET 9 SDK
- EF Core command line tools

# Setup

- Go to src\ShippingAcknowledgementWorker
- In appsettings.Development.json configure the path where the acknowledgement shipping notification will arrive and
  where the processed notification will be moved to

```json
"ShippingAcknowledgement": {
"Provider": {
"FilePath": "", <-- New acknowledgement shipping notification path
"ProcessedFilePath": "" <-- Processed acknowledgement shipping notification path
}
```

- Create a .db file for storing shipping acknowledgements

    ```cmd
    dotnet ef database update
    ```
- Run the application

    ```cmd
    dotnet run
    ```

# Usage

To use the application, drop the acknowledgement shipping notification in the path that was earlier configured.
The application will load the data every x seconds (by default, it's 5) into the SQLite database.

# Technical considerations

## New notification announcement

During analysis, I found there were two potential solutions, using

- FileSystemWatcher

  Pros:
    - Real-time notifications about changes in a directory.

  Cons:
    - According to the documentation, the OS notifies the component of file changes in a buffer that's created by the
      FileSystemWatcher and
      if there are many changes in a short time, the buffer can overflow and we can lose track of
      changes. https://learn.microsoft.com/en-us/dotnet/api/system.io.internalbufferoverflowexception?view=net-9.0
    - In case of downtime, any existing files would not be picked up
- Periodically scanning for files

  Pros:
    - The service would work even when a lot of files are being added in the directory
    - The service could pick up the missing files after a downtime

  Cons:
    - The files wouldn't be picked up instantly

For this solution, I went forward with **Periodically scanning for files** because if there isn't a requirement to react
to the
new notifications as fast as possible, I valued resiliency to a lot of files being uploaded simultaneously and to
downtime and the simplicity
that this solution provides.

## Notification processing

I analyzed that there are multiple options from which I found 2 that were suitable, using System.IO.Pipelines and
System.Threading.Channels.
I decided to move forward with System.Threading.Channels as it provides producer-consumer logic while also handling
objects and not bytes while
also being able to control how much the writer can write data.

System.IO.Pipelines would've been a more performant solution if implemented correctly, at the time I chose simplicity
over
performance, but if the current solution does not suffice, the solution is structured and decoupled in a way that makes
the change
not as difficult.

## Database interactions

For the simplicity I went forward with using SQLite and using AddRangeAsync by wrapping it in a transaction to avoid committing
each entity individually.

## File cleanup

When a notification is processed, it's moved to a processed path to avoid picking it up again and processing. This was
decided to preserve the original notification if it is necessary for auditing purposes. After that, it could be set to
be deleted after x amount of time. If preserving is not necessary, it should be deleted to preserve disk space, and
if there are any errors with the notification, it could be passed to an error path where it could be used for
troubleshooting.

# Technical flaws that should be fixed (in addition to existing comments)

- No input validations when reading the notification lines
- No retry policies if there are any transient errors with the database
- The database columns could be optimized to be more memory performance
- Error handling should be improved (mentioned in the comments)
- Startup configuration validations should be logged
- Before accessing the file, there should be a check in place if the file is currently locked
- Logging improvements in addition to error handling
- Clean architecture issue, accessibility modifiers should be adjusted to prohibit layers interacting with direct implementations
- Unique constraints in the database for entities that shouldn't have duplicates
- Code should be cleaned up in large files (especially ShippingAcknowledgementParser)
- Class property modifiers should be added to better reflect their behaviour if they are null or required