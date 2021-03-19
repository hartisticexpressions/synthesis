using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Synthesis.Util {
    public static class SynthesisUtil {

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> a) {
            foreach (var i in e) {
                a(i);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T, int> a) {
            for (int i = 0; i < e.Count(); i++) {
                a(e.ElementAt(i), i);
            }
        }
        
    }
}
