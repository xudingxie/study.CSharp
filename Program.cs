// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Task.Run(() => Console.WriteLine("132"));
Task myTask = new Task(() => { Console.WriteLine("my new Task"); });
myTask.Start();

myTask.ContinueWith((task) => Console.WriteLine("continueWith"));
await Task.Delay(1000 * 10);


