# Serilog.Sink.Http
Serilog.Sink.Http

## 使用

在Serilog配置中加入接收Uri配置即可进行推送

```C#
var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    //配置接收Uri
    .WriteTo.Http(new Uri("http://localhost:5268/logs"))
    .CreateLogger();
    
builder.Host.UseSerilog();
```
