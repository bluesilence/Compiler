using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Refer to http://blog.csdn.net/xuxurui007/article/details/18045943
namespace CART
{
    public abstract class InpurityMethod
    {

    }

    public class C4dot5 : InpurityMethod
    {
    }

    public abstract class FeatureType<T>
    {
        public FeatureType<T> GetSingleton();

        public bool Compare<T>(FeatureType<T> other);

        private FeatureType<T> singleton;
    }

    public class Nominal<T> : FeatureType<T>
    {
        Nominal(T val)
        {
            value = val;
        }

        public bool Compare<T>(Nominal<T> other)
        {
            if (value.Equals(other.value))
                return true;
            else
                return false;
        }

        private T value;
    }

    public class Ordinal<T> : FeatureType<T> where T : IComparable<T>
    {
        public Ordinal(T val)
        {
            value = val;
        }

        public bool Compare<T>(Ordinal<T> other) where T : IComparable<T>
        {
            if (value.CompareTo(other.value) > 0)
                return true;
            else
                return false;
        }

        private T value;
    }

    public class Numeric<T> : FeatureType<T> where T : IComparable<T>
    {   
        public override bool CompareTo<T>(T other) where T : IComparable<T>
        {
            if (value.CompareTo(target) > 0)
                return true;
            else
                return false;
        }
    }
    
    public struct Feature<T>
    {
        string featureName;
        FeatureType<T> featureType;

        Feature(string name, FeatureType<T> type)
        {
            featureName = name;
            featureType = type;
        }

        bool CompareValues<T>(ref T target, ref T value)
        {
            return featureType.GetSingleton().CompareValues<T>(ref target, ref value);
        }
    };

    public class DecisionTreeNode<T>
    {
        public DecisionTreeNode()
        {
            outcome = null;
        }

        public Feature<T> SplitFeature
        {
            get;
            private set;
        }

        public T SplitValue
        {
            get;
            private set;
        }

        //Valid only for leaf node
        public Nominal<T> outcome
        {
            get;
            private set;
        }

        public ArrayList[] FindBestSplit(ref ArrayList data, ref Feature<T>[] features, ref Dictionary<object, int> outcomeCounts)
        {
            ArrayList[] subsets = new ArrayList[2];

            int featureCount = features.Length;
            var outcomes = outcomeCounts.Keys;

            double originalInfo = CalcInfo(outcomeCounts);
            double[] featureInfo = new double[featureCount];

            for (int i = 0; i < featureCount; i++)
            {
                Dictionary<object, Dictionary<object, int>> outcomeCountPerValue = new Dictionary<object, Dictionary<object, int>>;
                for (int j = 0; j < data.Count; j++)
                {
                    ArrayList row = (ArrayList)data[j];
                    //Collect the outcome's count per value for this feature
                }

                featureInfo[i] = 0;
                foreach (int outcomeCount in outcomeCountPerValue.Values)
                {
                    //Calculate the count of each value of this feature & the total rows
                }

                foreach (object featureValue in outcomeCountPerValue.Keys)
                {
                    Dictionary<object, int> outcomeCount1Value = outcomeCountPerValue[featureValue];

                    featureInfo[i] += 1.0 * valueCount / totalRows * CalcInfo(outcomeCount1Value);
                }
            }

            //Calculate information gain ratio

            this.SplitFeature = features[0];
            this.SplitValue = (T)((ArrayList)data[0])[0];

            return subsets;
        }

        public void Terminate(ref ArrayList data, ref Dictionary<object, int> outcomeCounts)
        {
            left = null;
            right = null;
            leafData = data;

            object maxOutcome = from outcomeCount in outcomeCounts orderby outcomeCount.Value descending select outcomeCount.Key;

            this.outcome = maxOutcome;
        }

        public DecisionTreeNode<T> left { get; set; }
        public DecisionTreeNode<T> right { get; set; }

        private ArrayList leafData;
    }

    public class DecisionTree<T>
    {
        public DecisionTree(ref Feature<T>[] features)
        {
            this.features = features;
        }

        public DecisionTreeNode<T> Train(ref ArrayList data)
        {
            root = CreateNode(ref data);

            return root;
        }

        public List<Nominal<T>> Predict(ref ArrayList data);

        private DecisionTreeNode<T> CreateNode(ref ArrayList data)
        {
            if (data.Count == 0)
            {
                return null;
            }

            DecisionTreeNode<T> node = new DecisionTreeNode<T>();

            //To-Do: Add shrinkage as terminating condition
            Dictionary<object, int> outcomeCounts = CountOutcomes(ref data);
            if (outcomeCounts.Count == 1)
            {
                node.Terminate(ref data, ref outcomeCounts);
            }
            else
            {
                ArrayList[] subsets = node.FindBestSplit(ref data, ref features, ref outcomeCounts);

                node.left = CreateNode(ref subsets[0]);
                node.right = CreateNode(ref subsets[1]);
            }

            return node;
        }

        private Dictionary<object, int> CountOutcomes(ref ArrayList data)
        {
            Dictionary<object, int> outcomeCount = new Dictionary<object, int>();
            
            int outComeColumn = ((ArrayList)data[0]).Count - 1;

            foreach (ArrayList row in data)
            {
                outcomeCount[row[outComeColumn]]++;
            }

            return outcomeCount;
        }

        private DecisionTreeNode<T> root;

        private Feature<T>[] features;
    }
}
