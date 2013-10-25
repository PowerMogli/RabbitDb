RabbitDB
========

A simple abstraction of data access. Simple, feature-rich, clean and hopefully efficent. 
Works like to conjure a white rabbit out of a hat  :)

Simple
----------
No dynamics, simple and no obscure command execution. 
If used by convention - no attributes are needed.

Clean
-----
Based on clean-code and SOLID principles.

Features
------------
Change tracking, identity map, multiple resultsets, custom mappings
(all on request - soon to come).
Seperate `Entity` class to inherit from if you want to work with it without using within `DbSession`.

```csharp
[Table("Posts")]
class Post : Entity
{
    [Column("Id", DbType = DbType.Int32, AutoGenerated = true)]
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedOn { get; set; }
    public PostType Type { get; set; }
    public int? TopicId { get; set; }
    public bool IsActive { get; set; }
}
```
Let´s assume you want to use an other primary key then `ID`:
```charp
[Table("Posts", AlternativePKs="FirstID, SecondID")]
class Post
{
    public int Id { get; set; }
    public ....
}
```
If you decide to use alternative primary keys the default primary key will be ignored!!
And like you can see, you don´t have to use `Entity` class. You can use your own POCOs.

If you name your class like your table and your properties like your columns you don´t need attributes *at all*.
All needed information like name of table, name of column, dbtype, primarykey(s), default value, nullable is gathered by RabbitDB for you.

Loading data for entity:
```csharp
var post = new Post();
post.Id = 6;
post.Load();
```
Update entity (changes made to entity are tracked by RabbitDB for you). 
So if you change AuthorId to an other value then the one which was loaded, RabbitDB tracks this change and
executes an update command by calling `PersistChanges()` on the entity.
```csharp
var post = new Post();
post.Id = 6;
post.Load();

post.AuthorId = 444
post.PersistChanges();
```

Deletion:
```csharp
var post = new Post();
post.Id = 6;
post.PersistChanges(true); // true for executing delete command
```

If you decide to inherit from `Entity` you have to register your connection string and the used DbEngine.
```csharp
Registrar<string>.Register("Company.Module.*", @"YourConnectionString");
Registrar<DbEngine>.Register("Company.Module.*", DbEngine.SqlServer);
```

Let´s start using `DbSession`
```csharp
using (DbSession dbSession = new DbSession(@"YourConnectionString", DbEngine.MySql))
{
    ...
}

using (DbSession dbSession = new DbSession(@"YourConnectionString")) // SqlServer by default
{
    ...
}

using (DbSession dbSession = new DbSession(typeof(this))) // uses registered connection string and DbEngine for types namespace.
{
    ...
}
```
Self-explanatory command executions:
```charp
var scalarValue = dbSession.GetScalarValue<int>("SELECT ProductID FROM Products WHERE Name=@name", new { name = "Herbie" });

dbSsession.ExecuteCommand("UPDATE Posts SET IsActive=@0 WHERE Title=@1 and Id=@2", true, "Mark", 3);

// gets all table entries.
var objectSet = dbSession.GetObjectSet<Post>();

var objectSet = dbSession.GetObjectSet<Post>("SELECT * FROM Posts WHERE Title=@title OR Id=@id", new { title = "Mark", id = 5 });

// the same but without anonymous arguments
var objectSet = dbSession.GetObjectSet<Post>("SELECT * FROM Posts WHERE Title=@0 OR Id=@1", "Mark", 5);

// select your data by using lambda expressions.
var objectSet = dbSsession.GetObjectSet<Post>(post => post.Id == 4 || post.IsActive);
 
// gets the post with primary key '6'
var singleObject = dbSession.GetObject<Post>(6);

// gets all posts which fulfill this lambda expression criteria
var singleObject = dbSession.GetObject<Post>(post => post.Title == "Mark" && post.Id == 6); 

// gets a single value
var value = dbSession.GetValue<int>("SELECT COUNT(*) FROM Posts");

// gets a set of values
var titles = dbSession.GetObjectSet<string>("SELECT Title FROM Posts");
```
Stored procedures:
```csharp
dbSession.ExecuteStoredProcedure("yourStoredProcedureName", new { pParam1 = "Fred", pParam2 = PostType.Page });

// or to receive your poce with a stored procedure
dbSession.ExecuteStoredProcedure<YourPOCO>("yourStoredProcedureName", new { pParam1 = "Fred", pParam2 = PostType.Page });
```
Or if you have a stored procedure with a lot of parameters:
```csharp
public class ExampleProcedure : SqlStoredProcedure
{
    public ExampleProcedure()
        : base("yourStoredProcedureNameHere") { }

    public string TicketID
    {
        get { return base.GetParameterValue<string>("@pTicketID"); }
        set { base.AddParameter("@pTicketID", value, DbType.AnsiString, 255); }
    }
}

// Any where else

ExampleProcedure procedure = new ExampleProcedure();
procedure.Execute();
```
Or within a `DbSession`:
```csharp
using (DbSession dbSession = new DbSession("YourConnectionStringHere", DbEngine.SqlServer))
{
    ExampleProcedure procedure = new ExampleProcedure();
    dbSession.ExecuteStoredProcedure(procedure);
}
```
