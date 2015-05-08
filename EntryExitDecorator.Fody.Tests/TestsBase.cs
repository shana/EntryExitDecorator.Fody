using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xunit;

namespace EntryExitDecorator.Fody.Tests {
    public abstract class TestsBase : IDisposable {
        private readonly static object _sync = new object();

        protected abstract Assembly Assembly { get; }

        protected abstract dynamic RecordHost { get; }

        protected IList<Tuple<Method, object[]>> Records {
            get {
                var records = (IList<Tuple<int, object[]>>)this.RecordHost.Records;
                return records.Select(x => new Tuple<Method, object[]>((Method)x.Item1, x.Item2)).ToList();
            }
        }

        protected TestsBase() {
            // almost global lock to prevent parrllel test run because we use static (
            Monitor.Enter(_sync);
        }
        
        protected void CheckMethodSeq(Method[] methods) {
            var coll = this.Records.Select(x => x.Item1).ToArray();
            Assert.Equal(methods, coll);
        }

        protected void CheckMethodNotCalledSeq(Method[] methods)
        {
            var coll = this.Records.Select(x => x.Item1).ToArray();
            var coll2 = coll.Where(x => !methods.Contains(x)).ToArray();
            Assert.Equal(coll2, coll);
        }

        private Tuple<Method, object[]> GetRecordOfCallTo(Method method)
        {
            var record = this.Records.SingleOrDefault(x => x.Item1 == method);
            if (record == null)
            {
                throw new InvalidOperationException(method+" was not called.");
            }
            return record;
        }

        protected void CheckBody(string methodName, string extraInfo = null) {
            Assert.True(this.Records.Any(x => x.Item1 == Method.Body &&
                                              x.Item2[0] == methodName &&
                                              x.Item2[1] == extraInfo));
        }

        protected void CheckEntry() {
            Assert.True(this.Records.Any(x=>x.Item1 == Method.OnEnter));
        }

        protected void CheckExit() {
            Assert.True(this.Records.Any(x => x.Item1 == Method.OnExit));
        }

        public void Dispose() {
            Monitor.Exit(_sync);
        }
    }
}