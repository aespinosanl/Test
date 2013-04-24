﻿using System;
using System.Threading.Tasks;
using Nfield.Infrastructure;
using Nfield.Services;
using Ninject;

namespace Nfield.SDK.Samples
{

    /// <summary>
    /// Sample application for demonstrating Nfield SDK usage.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Example of using the Nfield SDK with a user defined IoC container.
            // In most cases the IoC container is created and managed through the application. 
            using(IKernel kernel = new StandardKernel())
            {
                InitializeNfield(kernel);

                const string serverUrl = "https://manager.nfieldmr.com/";

                // First step is to get an INfieldConnection which provides services used for data access and manipulation. 
                INfieldConnection connection = NfieldConnectionFactory.Create(new Uri(serverUrl));

                // User must sign in to the Nfield server with the appropriate credentials prior to using any of the services.
                connection.SignInAsync("testdomain", "user1", "password123").Wait();

                // Request the Interviewers service to manage interviewers.
                INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();

                NfieldInterviewersManagement interviewersManager = new NfieldInterviewersManagement(interviewersService);

                // Perform synchronous and asynchronous operations on Interviewers.
                Task t1 = interviewersManager.AddInterviewerAsync();
                interviewersManager.AddInterviewer();
                
                Task t2 = interviewersManager.UpdateInterviewerAsync();
                interviewersManager.UpdateInterviewer();

                Task.WaitAll(t1, t2);

                interviewersManager.QueryForInterviewers();
                interviewersManager.QueryForInterviewersAsync();
                
                interviewersManager.RemoveInterviewerAsync().Wait();
                interviewersManager.RemoveInterviewer(); 
            }
        }

        /// <summary>
        /// Example of initializing the SDK with Ninject as the IoC container.
        /// </summary>
        private static void InitializeNfield(IKernel kernel)
        {
            DependencyResolver.Register(type => kernel.Get(type), type => kernel.GetAll(type));

            NfieldSdkInitializer.Initialize((bind, resolve) => kernel.Bind(bind).To(resolve).InTransientScope(),
                                            (bind, resolve) => kernel.Bind(bind).To(resolve).InSingletonScope(),
                                            (bind, resolve) => kernel.Bind(bind).ToConstant(resolve));
        }
    }
}