using ConsoleApp1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CreateHostBuilder(args).Build().Run();

KafkaDependentProducer<string, string> kafkaDependentProducer =
    new KafkaDependentProducer<string, string>(new KafkaClientHandle(new ConfigurationManager()));

static IHostBuilder CreateHostBuilder(string[] args) => 
    Host.CreateDefaultBuilder(args).ConfigureServices((context, collection) =>
    {
        collection.AddHostedService<KafkaConsumerHostedService>();
        collection.AddHostedService<KafkaProducerHostedService>();
    });
