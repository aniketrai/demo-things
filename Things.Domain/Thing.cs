using System;

namespace Things.Domain
{
    /// <summary>
    /// Thing model class.
    /// </summary>
    public class Thing
    {
        /// <summary>
        /// Gets or Sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or Sets Name.
        /// </summary>
        public string Name { get; set; }
    }
}
