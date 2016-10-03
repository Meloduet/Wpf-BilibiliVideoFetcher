# Aria2Controler

  封装了JSON-RPC调用Aria2进行下载相关操作的方法

## Requirement

+ OS: Windows 7+
+ .NETFramework 4.5+
+ Visual Studio 2015

## Usage

### 添加下载

#### 下载百度主页网页源码

```CSharp
// gid是Aria2中每个下载任务的标识
var gid = Aria2Helper.AddUri("http://www.baidu.com");
```

#### 下载百度主页网页源码，并保存到`D:\`下，文件名为`1.txt`，使用3个连接（默认为5个）

```CSharp
string gid = Aria2Helper.AddUri(
  "http://www.baidu.com",
    new Dictionary<string, string>() {
          ["dir"] = "D:\\",
          ["out"] = "1.txt",
          ["split"] = "3",
          ["max-connection-per-server"] = "3"
          });
```

### 查询任务状态

```CSharp
// 获取所有在在下载的任务
Aria2Helper.TellActive();

// 获取前1000个已暂停的下载任务，不足1000则返回实际数目
Aria2Helper.TellStopped(0,1000);

// 获取前1000个处于下载队列中，并且并不处于激活状态的下载任务，不足1000则返回实际数目
Aria2Helper.TellWaiting(0,1000);
```

### 暂停、恢复任务

```CSharp
// 暂停所有下载任务
Aria2Helper.PauseAll();

// 暂停指定gid对应的任务
Aria2Helper.Pause(gid);

// 恢复指定gid对应的任务
var r = Aria2Helper.Unpause(gid);

// 恢复所有下载任务
Aria2Helper.UnpauseAll();
```

### 调整任务顺序

```CSharp
// 移动到下载队列首位
Aria2Helper.ChangePosition(gid, 0, PositionOrigin.Begin);

// 移动到下载队列第二位
Aria2Helper.ChangePosition(gid, 1, PositionOrigin.Begin);

// 向前移动一位
Aria2Helper.ChangePosition(gid, -1, PositionOrigin.Current);

// 移动到下载队列队尾
Aria2Helper.ChangePosition(gid, 0, PositionOrigin.End);

```

### 主动移除任务

```CSharp
Aria2Helper.Remove(gid);
```

### ETC

[Aria2Helper单元测试文件中](../UnitTestFetcher/TestAria2Helper.cs)

## Third party dependencies

+ [Newtonsoft.Json](http://www.newtonsoft.com/json)