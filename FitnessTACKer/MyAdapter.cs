using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace FitnessTACKer
{
    class MyAdapter : RecyclerView.Adapter, IFilterable
    {
        private JavaList<WorkoutItem> data;
        private readonly JavaList<WorkoutItem> currentList;
        public event EventHandler<int> ItemClick;

        public MyAdapter(JavaList<WorkoutItem> data)
        {
            this.data = data;
            this.currentList = data;
        }
        //BIND DATA TO VIEWS
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyViewHolder h = holder as MyViewHolder;

            if (h != null)
            {
                h.NameTxt.Text = data[position].title;
                h.ExTxt.Text = data[position].exercises;
            }

            if (data[position].expanded)
            {
                // expand
                h.ExpandedLayout.Visibility = ViewStates.Visible;
            }
            else //collapse
                h.ExpandedLayout.Visibility = ViewStates.Gone;
        }
        //INITIALIZE VH
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //INFLATE LAYOUT TO VIEW
            View v = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Model, parent, false);
            MyViewHolder holder = new MyViewHolder(v, OnClick);

            return holder;
        }

        public override int ItemCount
        {
            get { return data.Count; }
        }

        public void setData(JavaList<WorkoutItem> filteredData)
        {
            this.data = filteredData;
        }
        public Filter Filter
        {
            get { return FilterHelper.newInstance(currentList, this); }
        }

        private void OnClick(int position)
        {
            if (ItemClick != null)
            {
                ItemClick(this, position);
            }
        }


        /*
         * Our ViewHolder class
         */
        internal class MyViewHolder : RecyclerView.ViewHolder
        {
            public TextView NameTxt;
            public LinearLayout ExpandedLayout;
            public TextView ExTxt;
            public MyViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                NameTxt = itemView.FindViewById<TextView>(Resource.Id.nameTxt);
                ExpandedLayout = itemView.FindViewById<LinearLayout>(Resource.Id.expanded_wo);
                ExTxt = itemView.FindViewById<TextView>(Resource.Id.ex_exercises);
                itemView.Click += (sender, e) => listener(base.AdapterPosition);
                NameTxt.Click += (sender, e) => listener(base.AdapterPosition);
            }
        }
    }
}