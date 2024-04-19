学习使用Task

对于Task的理解初步的认识是开启一个异步的子线程来完成功能。比较常用的也就是`await Task.Delay(1000);`这种用于延迟的操作。但是后面在学习中发现这个功能应该非常强大。用好是嘎嘎好用的。可以非常好的简化逻辑。

接下来我就用这么几个方法来完善自己对于Task的学习。

## Task.Run(()=>{})与Task.Start()

我为什么要把这两个放在一起。因为这是我最先接触到的Task的用法。

```C#
Task.Run(() => Console.WriteLine("132"));
Task myTask = new Task(() => { Console.WriteLine("my new Task"); });
myTask.Start();
```

这两种都可以执行简单的异步方法。所以区别就是执行的实际。第一种run方法是立刻执行的。然而如果是使用返回的话就需要start的方法来手动触发执行。

## Task.Delay(1000*10)

比较常用的延迟的方式。注意，需要配置await一起使用，才是在当前位置进行暂停。否则是另外开辟线程进行执行。是否可以用这个来判断是否超时啊。

```C#
Task.Delay(1000*10);
```

## myTask.ContinueWith(t=>{})

在myTask的任务完成之后执行。在出现异常结果后也可以使用t进行判断来执行不同的结果。

看到比较心动的方法。其中t是必要的参数，就是myTask这个Task类型的参数。

比较容易想到的应用就是在设备连接成功之后去读取相应的触发信号，这样在多线程的情况下就不用等待前面的内容全部完成了。并且逻辑更加清晰，便于理解。

特性：

- 无论myTask任务被取消，成功，失败都会继续执行下面的任务。可以根据t的状态来执行不同的任务。
- 如果myTask任务已经完成，会立刻执行下个任务。如果没有完成，则会等待完成之后继续执行下面内容。似乎也可以理解成追加的任务是ok的。

```C#
Task myTask = new Task(() => { Console.WriteLine("my new Task"); });
myTask.Start();
myTask.ContinueWith((task) => Console.WriteLine("continueWith"));
```

## 有返回值的Task\<T>



最常见的Task\<T>的类型

```c#
Task<int> firstTask = Task.Run(() =>
{
    // 这是第一个任务，返回一个整数值
    return 5;
});
// 在第一个任务完成后执行后续任务
string finalResult = await firstTask.ContinueWith(prevTask =>
{
    // 获取前一个任务的返回值
    int result = prevTask.Result;

    // 使用前一个任务的返回值进行一些操作
    return $"Previous task returned: {result}";
});
Console.WriteLine(finalResult); // 输出: Previous task returned: 5
```

在这个示例中出现了两种获取返回值的方式。`task.result`与`await task`这两种都是可以获取到正确的结果。并且result也是堵塞当前线程的。实际上在体验上与await一样阻塞当前线程。另外，如果在task中抛出了异常，那样result会使用`AggregateException`把原始的异常重新包装，但是await不会，他只是默默把自己添加到堆栈中。

综上`只推荐使用await`获取最后的结果的值。

