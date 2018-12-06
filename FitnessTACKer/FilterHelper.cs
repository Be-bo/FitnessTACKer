/*
- http://camposha.info/source/xamarin-android-s1e7-recyclerview-searchfilter
*/

using Android.Runtime;
using Android.Widget;
using Java.Lang;

namespace FitnessTACKer
{

    class FilterHelper : Filter
    {
        static JavaList<WorkoutItem> currentList;
        static MyAdapter adapter;

        public static FilterHelper newInstance(JavaList<WorkoutItem> currentList, MyAdapter adapter)
        {
            FilterHelper.adapter = adapter;
            FilterHelper.currentList = currentList;
            return new FilterHelper();
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            FilterResults filterResults = new FilterResults();
            if (constraint != null && constraint.Length() > 0)
            {
                //CHANGE TO UPPER
                //constraint = constraint.ToString().ToUpper();
                string query = constraint.ToString().ToUpper();

                //HOLD FILTERS WE FIND
                JavaList<WorkoutItem> foundFilters = new JavaList<WorkoutItem>();

                //ITERATE CURRENT LIST
                for (int i = 0; i < currentList.Count; i++)
                {
                    string title = currentList[i].title;

                    //SEARCH
                    if (title.ToUpper().Contains(query.ToString()))
                    {
                        //ADD IF FOUND
                        foundFilters.Add(currentList[i]);
                    }
                }
                //SET RESULTS TO FILTER LIST
                filterResults.Count = foundFilters.Count;
                filterResults.Values = foundFilters;
            }
            else
            {
                //NO ITEM FOUND.LIST REMAINS INTACT
                filterResults.Count = currentList.Count;
                filterResults.Values = currentList;
            }

            //RETURN RESULTS
            return filterResults;
        }
        /*
         * Publish results to UI.
         */
        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            adapter.setData((JavaList<WorkoutItem>)results.Values);
            adapter.NotifyDataSetChanged();
        }
    }
}
