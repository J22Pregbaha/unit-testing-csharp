using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class StackTests
    {
        private Stack<string> _stack;

        [SetUp]
        public void Setup()
        {
            _stack = new Stack<string>();
        }
        
        [Test]
        public void Push_ArgIsNull_ReturnArgumentNullException()
        {
            Assert.That(() => _stack.Push(null), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Push_ArgIsNotNull_AddToStack()
        {
            _stack.Push("a");
            
            Assert.That(_stack.Count == 1);
        }

        [Test]
        public void Count_StackIsEmpty_ReturnZero()
        {
            Assert.That(_stack.Count == 0);
        }

        [Test]
        public void Pop_StackIsEmpty_ReturnInvalidOperationException()
        {
            Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_StackIsNotEmpty_ReturnTopElementOfStack()
        {
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            var result = _stack.Pop();
            
            Assert.That(result == "c");
        }

        [Test]
        public void Pop_StackIsNotEmpty_RemoveTopElementOfStack()
        {
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            _stack.Pop();
            
            Assert.That(_stack.Count == 2);
        }

        [Test]
        public void Peek_StackIsEmpty_ReturnInvalidOperationException()
        {
            Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
        }

        [Test]
        public void Peek_StackIsNotEmpty_ReturnTopElementOfStack()
        {
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            var result = _stack.Peek();
            
            Assert.That(result == "c");
        }

        [Test]
        public void Peek_StackIsNotEmpty_DoesNotRemoveTopElementOfStack()
        {
            _stack.Push("a");
            _stack.Push("b");
            _stack.Push("c");

            _stack.Peek();
            
            Assert.That(_stack.Count == 3);
        }
    }
}