using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    public class Logger
    {
        #region Data
        
		private static object _lock = new object();
        private static Logger _logger = null;

        public static Logger Instance
        {
            get 
            {
                // If this is the first time were referring to the
                // singleton object, the private variable will be null.
                if (_logger == null)
                {
                    // for thread safety, lock an object when
                    // instantiating the new Logger object. This prevents
                    // other threads from performing the same block at the
                    // same time.
                    lock (_lock)
                    {
                        // Two or more threads might have found a null
                        // mLogger and are therefore trying to create a 
                        // new one. One thread will get to lock first, and
                        // the other one will wait until mLock is released.
                        // Once the second thread can get through, mLogger
                        // will have already been instantiated by the first
                        // thread so test the variable again. 
                        if (_logger == null)
                            _logger = new Logger();
                    }
                }
				
                return _logger;
            }
        }

        private List<ILogger> _observers;

		#endregion Data

        #region Constructor
		
        private Logger()
        {
            _observers = new List<ILogger>();
        }
		
		#endregion Constructor

        #region Public Methods
		
        public void RegisterObserver(ILogger observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add (observer);
        }

        public void AddLogMessage(string message)
        {
            // Apply some basic formatting like the current timestamp
            var formattedMessage = string.Format("{0} - {1}", DateTime.Now.ToString(), message);
			
            foreach (var observer in _observers)
            	observer.ProcessLogMessage(formattedMessage);
		}

        public void AddLogMessage(Exception ex)
        {
            var message = new StringBuilder(ex.Message);
            var trace = ex.StackTrace;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message.Append("\n" + ex.Message);
            }
			
            message.Append("\nStack trace: " + trace);

            AddLogMessage(message.ToString());
        }
		
		#endregion Public Methods
    }
}