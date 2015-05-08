using System;
using System.Collections.Generic;

namespace SimpleTest {
    public enum Method {
        OnEnter = 1,
        Body = 2,
        OnExit = 3,
    }

    public static class TestRecords {
        private static readonly IList<Tuple<int, object[]>> _records = new List<Tuple<int, object[]>>();

        public static void Clear() {
            _records.Clear();
        }

        public static void RecordOnEntry() {
            Record(Method.OnEnter);
        }

        public static void RecordOnExit() {
            Record(Method.OnExit);
        }

        public static void RecordBody(string name, string extraInfo = null) {
            Record(Method.Body, new object[] { name, extraInfo });
        }

        public static void Record(Method method, object[] args = null) {
            _records.Add(new Tuple<int, object[]>((int)method, args));
        }

        public static IList<Tuple<int, object[]>> Records {
            get { return _records; }
        }
    }
}