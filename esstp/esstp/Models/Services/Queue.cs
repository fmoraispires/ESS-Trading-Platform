using System;
using System.Collections.Generic;
using System.Threading;
using esstp.Models.ViewModels;

namespace esstp.Models.Services
{

    public class Queue 
    {
      

        private List<MarketViewModel> data = new List<MarketViewModel>();
        
        public void Push(MarketViewModel mkt)
        {
            lock (data)
            {
                data.Add(mkt);
                Monitor.Pulse(data); //notify();
            }

        } //push(TroubleTicket)


        public MarketViewModel Pull()
        {
            lock (data)
            {
                while (data.Count == 0)
                {
                    try
                    {
                        Monitor.Wait(data); //Wait();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e);
                    } //try
                } //while
                MarketViewModel mkt = data[0];
                data.RemoveAt(0);
                return mkt;
            }
        } //pull()

        public int Size()
        {
            return data.Count;
        } //size()

    } //class Queue
}
