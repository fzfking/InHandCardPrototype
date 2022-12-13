using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Infrastructure.Interfaces
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator enumerator);
        public void StopCoroutine(Coroutine coroutine);
    }
}