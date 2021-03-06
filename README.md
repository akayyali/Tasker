# Tasker 
Is an In-Memory lighweight recurring Tasks scheduler.
it allows you to :

*Add

*Remove

*Start

*Stop

and Schedule Jobs to be run on extact date and time.

Nuget package: https://www.nuget.org/packages/inmemtasker

Adding a new Scheduler Service is very simple :

First make sure you DI your scheduler service

            services.AddSingleton<ApplicationJobSchedulerService>();
            //DI your jobs only if you want inject them
            services.AddTransient<PingExternalDependencyJob>(sp=> { return new PingExternalDependencyJob(sp) { RunEvery = TimeSpan.FromSeconds(10), Payload = "http://google.com"}; });

Then register your SchedulerService

            services.AddHostedService<ApplicationJobSchedulerService>(sp =>
            {
                var srv = sp.GetRequiredService<ApplicationJobSchedulerService>();
                
                //add any jobs you want here
                srv.AddJob(new CheckSystemHealthJob(sp)
                {
                    RunEvery = TimeSpan.FromSeconds(5),
                    Payload = "All systems are running smoothly"
                }, true);


                return srv;
            });
            
You can also add jobs dynamically from within the scheduler service

          public class ApplicationJobSchedulerService : JobSchedulerService
              {
                  public ApplicationJobSchedulerService(IServiceProvider serviceProvider)
                  {
                      var Job = new CheckSystemHardwareJob(serviceProvider)
                      {
                          RunEvery = TimeSpan.FromSeconds(10),
                          Payload = "Hardware in excellent condition"
                      };
                      AddJob(Job, DateTime.Now.AddSeconds(30));

                      //you can fetch jobs from external sources like db, or web resources and add them here
                  }
              }
