using System;
using System.Collections.Generic;
using UnityEngine; // TODO wrap Unity Random. Script should be engine agnostic.

namespace AI_Midterm_RTS.AICore.Distributions
{
    /// <summary>
    /// A weighted table of objects with adjustable coefficients.
    /// </summary>
    /// <typeparam name="T">The objects in the table.</typeparam>
    public class WeightedTable<T> : IDistribution<T>
    {
        #region Table Fields
        private List<float> weights;
        private List<float> coefficients;
        private List<T> results;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new weighted table with the given weights and results.
        /// </summary>
        /// <param name="weights">The weights for the results to occur.</param>
        /// <param name="results">The results tied to each weight.</param>
        public WeightedTable(List<float> weights, List<T> results)
        {
            // Check for invalid input.
            if (weights.Count != results.Count)
                throw new ArgumentException("Weights and result must be the same size.", "weights");
            // Don't allow for negative weights because it breaks the table.
            for (int i = 0; i < weights.Count; i++)
                if (weights[i] < 0f)
                    weights[i] = 0f;
            // Initialize fields.
            // TODO these should copy, not set directly.
            // caller could change these causing bad state.
            this.weights = weights;
            this.results = results;
            this.coefficients = new List<float>(weights.Count);
            // Set all coefficients to one by default.
            ResetCoefficients();
        }
        /// <summary>
        /// Creates a new empty weighted table.
        /// </summary>
        public WeightedTable() : this(new List<float>(), new List<T>())
        {

        }
        #endregion
        #region IDistribution Implementation
        /// <summary>
        /// Gets the next pull from the table based on weights.
        /// </summary>
        /// <returns>The next outcome.</returns>
        public T Next()
        {
            // Catch a pull that is done before
            // anything is added to the table.
            if (results.Count == 0)
                throw new Exception("Cannot pull from table with no elements!");
            // Get a random value in the weighted ranges.
            float randomValue = CalculateTotalWeight() * UnityEngine.Random.value;
            // Search for the weighted range it falls in
            // using an accumulator.
            float accumulator = 0f;
            for (int i = 0; i < weights.Count; i++)
            {
                accumulator += weights[i] * coefficients[i];
                if (accumulator > randomValue)
                    return results[i];
            }
            // If no result was hit return the last.
            // (might happen in the case of float roundoff errors)
            return results[results.Count - 1];
        }
        #endregion
        #region Add + Remove Results
        /// <summary>
        /// Adds a new result to the table.
        /// </summary>
        /// <param name="weight">The weight for the new item.</param>
        /// <param name="result">The result linked to the weight.</param>
        public void AddResult(float weight, T result)
        {
            // Check for invalid weight.
            if (weight < 0f)
                weight = 0f;
            // Add the new element.
            weights.Add(weight);
            results.Add(result);
            coefficients.Add(1f);
        }
        /// <summary>
        /// Removes a result from the table if it exists in the table.
        /// </summary>
        /// <param name="result">The result to remove.</param>
        public void RemoveResult(T result)
        {
            int index = results.FindIndex(0, (item) => item.Equals(result));
            if (index != -1)
            {
                weights.RemoveAt(index);
                coefficients.RemoveAt(index);
                results.RemoveAt(index);
            }
        }
        #endregion
        #region Modify Weights
        /// <summary>
        /// Sets the weight of a result if it exists in the table.
        /// </summary>
        /// <param name="result">The result to adjust.</param>
        /// <param name="weight">The new weight of the result.</param>
        public void SetWeight(T result, float weight)
        {
            // Check for invalid weight.
            if (weight < 0f)
                weight = 0f;
            // Try to set the weight.
            int index = results.FindIndex(0, (item) => item.Equals(result));
            if (index != -1)
                weights[index] = weight;
        }
        #endregion
        #region Modify Coefficients
        /// <summary>
        /// Sets the coefficient for a result.
        /// </summary>
        /// <param name="result">The result to augment.</param>
        /// <param name="coefficient">A multiplier on this result's weight.</param>
        public void SetCoefficient(T result, float coefficient)
        {
            // Check for invalid coefficient.
            if (coefficient < 0f)
                coefficient = 0f;
            // Try to set the coefficient.
            int index = results.FindIndex(0, (item) => item.Equals(result));
            if (index != -1)
                coefficients[index] = coefficient;
        }
        /// <summary>
        /// Resets all coefficients to one.
        /// </summary>
        public void ResetCoefficients()
        {
            for (int i = 0; i < coefficients.Count; i++)
                coefficients[i] = 1f;
        }
        #endregion
        #region Predictive Methods
        /// <summary>
        /// Calculates the chance that a given outcome will occur.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A value between 0 and 1 representing the likeliness of this happening.</returns>
        public float ChanceOf(T result)
        {
            // If this result exists, check its weight over the total weight.
            int index = results.FindIndex(0, (item) => item.Equals(result));
            if (index != -1)
            {
                float totalWeight = CalculateTotalWeight();
                if (totalWeight != 0f)
                    return weights[index] * coefficients[index] / totalWeight;
            }
            // Otherwise assume this result can never happen.
            return 0f;
        }
        #endregion
        #region Retrieve Clone
        /// <summary>
        /// Clones this weighted table.
        /// </summary>
        /// <returns>An independent copy of the table.</returns>
        public WeightedTable<T> Clone()
        {
            // Create a new table to copy into.
            WeightedTable<T> clone = new WeightedTable<T>();
            // Clone the values for the underlying table.
            clone.weights = new List<float>();
            clone.results = new List<T>();
            clone.coefficients = new List<float>();
            for (int i = 0; i < weights.Count; i++)
            {
                clone.weights.Add(weights[i]);
                clone.results.Add(results[i]);
                clone.coefficients.Add(coefficients[i]);
            }
            // Return the new unassociated clone.
            return clone;
        }
        #endregion
        #region Utility Functions
        private float CalculateTotalWeight()
        {
            // Sum up and return all weights accounting
            // for the current coefficients.
            float totalWeight = 0f;
            for (int i = 0; i < weights.Count; i++)
                totalWeight += weights[i] * coefficients[i];
            return totalWeight;
        }
        #endregion
    }
}
