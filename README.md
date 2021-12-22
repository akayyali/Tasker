# Tasker 
Is an In-Memory lighweight recurring Tasks scheduler.
it allows you to :

<ul>
<li>Add</li>
<li>Remove</li>
<li>Start</li>
<li>Stop</li>

</ul>

and Schedule Jobs to be run on extact date and time.

Nuget package: https://www.nuget.org/packages/inmemtasker

Adding a new Scheduler Service is very simple :

Start by instaling nuget package using:

`Install-Package InMemTasker`

Then Make sure you DI your scheduler service
``` CSHarp
            services.AddSingleton<ApplicationJobSchedulerService>();
            //DI your jobs only if you want inject them
            services.AddTransient<PingExternalDependencyJob>(sp=> { return new PingExternalDependencyJob(sp, TimeSpan.FromSeconds(10), "http://google.com"); });
```
Then register your SchedulerService
``` CSHarp
            services.AddHostedService<ApplicationJobSchedulerService>(sp =>
            {
                var srv = sp.GetRequiredService<ApplicationJobSchedulerService>();
                
                //add any jobs you want here
                srv.AddJob(new CheckSystemHealthJob(sp, TimeSpan.FromSeconds(3),"All systems are running smoothly" ), true);

                return srv;
            });
```            
You can also add jobs dynamically from within the scheduler service

``` CSHarp
          public class ApplicationJobSchedulerService : JobSchedulerService
              {
                  public ApplicationJobSchedulerService(IServiceProvider serviceProvider)
                  {
                      var Job = new CheckSystemHardwareJob(serviceProvider, TimeSpan.FromSeconds(30), "Hardware in excellent condition");
                      Job.Executing += Job_Executing;
                      
                      //Set Job to Kick on a specfic day and time of week.
                      AddJob(Job, new StartOnNextDayOfWeek() { DayOfWeek = DayOfWeek.Saturday, Hour = 15, Minute = 30, Second = 15 });

                      //you can fetch jobs from external sources like db, or web resources and add them here
                  }
                  private void Job_Executing(object sender, JobExecutingEventArgs e)
                    {
                        //e.Skip = true;
                    }
              }
```
