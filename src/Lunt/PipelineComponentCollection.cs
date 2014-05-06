using System.Collections;
using System.Collections.Generic;
using Lunt.Runtime;

namespace Lunt
{
    /// <summary>
    /// Provides a mechanism for enumerating Lunt components.
    /// </summary>
    public sealed class PipelineComponentCollection : IPipelineComponentCollection
    {
        private readonly List<IPipelineComponent> _components;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineComponentCollection" /> class.
        /// </summary>
        /// <param name="components">The components.</param>
        public PipelineComponentCollection(IEnumerable<IPipelineComponent> components)
        {
            _components = new List<IPipelineComponent>(components);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineComponentCollection" /> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        public PipelineComponentCollection(IPipelineScanner scanner)
        {
            _components = new List<IPipelineComponent>(scanner.Scan());
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{ILuntComponent}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<IPipelineComponent> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}