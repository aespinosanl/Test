<h1>NIPO Software Nfield SDK for Windows and .NET 4.0</h1>
<p>This SDK allows you to build applications that take advantage of the Nfield services.</p>

<h1>Features</h1>
<ul>
    <li>Interviewers
        <ul>
            <li>Create/List/Update/Delete interviewers</li>
        </ul>
    </li>
</ul>
        
<h1>Requirements</h1>
<ul>
    <li>.NET Framework 4.0 or later</li>
    <li>To use this SDK to call Nfield services you need an Nfield account.</li>
</ul>

<h1>Usage</h1>
<p>To get the source code of the SDK clone this repository and include the <code>Library</code> project in your solution.</p>

<h1>Code Samples</h1>
<p>A comprehensive sample project can be found in the <code>Examples</code> folder.</p>
<p>The basic required steps are shown below.</p>
<p>First we need to use a dependency resolver.
<pre>using(IKernel kernel = new StandardKernel()) {</pre>
</p>
<pre>            DependencyResolver.Register(type => kernel.Get(type), type => kernel.GetAll(type));
            NfieldSdkInitializer.Initialize((bind, resolve) => kernel.Bind(bind).To(resolve).InTransientScope(),
                                            (bind, resolve) => kernel.Bind(bind).To(resolve).InSingletonScope(),
                                            (bind, resolve) => kernel.Bind(bind).ToConstant(resolve));
</pre>
<pre>INfieldConnection connection = NfieldConnectionFactory.Create(new Uri(serverUrl));</pre>
<pre>connection.SignInAsync("testdomain", "user1", "password123").Wait();</pre>
<pre>INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();</pre>
<pre>            Interviewer interviewer = new Interviewer
            {
                ClientInterviewerId = "ftropo5i",
                FirstName = "Bill",
                LastName = "Gates",
                EmailAddress = "bill@hotmail.com",
                TelephoneNumber = "0206598547",
                UserName = "bill",
                Password = "password12"
            };

            await _interviewersService.AddAsync(interviewer);
</pre>



<h1>Feedback</h1>
<p>For feedback related to this SDK please visit the
<a href="http://www.nfieldmr.com/">Nfield website</a>.</p>
