
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace FitnessTACKer
{
    public class Keyboard : LinearLayout, Java.IO.ISerializable{

        private SparseArray<String> keyValues = new SparseArray<String>();
        private InputConnection inputConnection;
        // keyboard keys (buttons)
        private Button mButton1;
        private Button mButton2;
        private Button mButton3;
        private Button mButton4;
        private Button mButton5;
        private Button mButton6;
        private Button mButton7;
        private Button mButton8;
        private Button mButton9;
        private Button mButton0;
        private ImageButton plate45_button;
        private ImageButton plate35_button;
        private ImageButton plate25_button;
        private ImageButton plate10_button;
        private ImageButton plate5_button;
        private ImageButton plate2_5_button;
        private ImageButton barbell_button;
        private ImageButton ez_button;
        private ImageButton hide_keyboard;
        private Button button_clear;
        private Button button_done;
        private ImageButton button_delete;
        private int plate45 = 0;
        private int plate35 = 0;
        private int plate25 = 0;
        private int plate10 = 0;
        private int plate5 = 0;
        private int plate2_5 = 0;
        private int barbell = 0;
        private int ez = 0;
        public View keyboard;
        private EditText editText;
       
        public Keyboard():
        base(null){}

        public Keyboard(Context context) :
            this(context, null, 0){
        }

        public Keyboard(Context context, IAttributeSet attrs) :
            this(context, attrs, 0){
        }

        public Keyboard(Context context, IAttributeSet attrs, int defStyleAttr):
            base(context, attrs, defStyleAttr){
            init(context, attrs);
        }

        public void setCurrentEditText(EditText ed)
        {
            if (editText != null) { 
            this.editText.SetBackgroundResource(Resource.Drawable.input_box_background);
            }
            this.editText = ed;
            editText.SetBackgroundResource(Resource.Drawable.weight_outline);
        }

        public void init(Context context, IAttributeSet attrs){
            //initialize buttons
            keyboard = LayoutInflater.From(context).Inflate(Resource.Layout.keyboard, this, true);
            mButton1 = FindViewById<Button>(Resource.Id.button_1);
            mButton2 = FindViewById<Button>(Resource.Id.button_2);
            mButton3 = FindViewById<Button>(Resource.Id.button_3);
            mButton4 = FindViewById<Button>(Resource.Id.button_4);
            mButton5 = FindViewById<Button>(Resource.Id.button_5);
            mButton6 = FindViewById<Button>(Resource.Id.button_6);
            mButton7 = FindViewById<Button>(Resource.Id.button_7);
            mButton8 = FindViewById<Button>(Resource.Id.button_8);
            mButton9 = FindViewById<Button>(Resource.Id.button_9);
            mButton0 = FindViewById<Button>(Resource.Id.button_0);
            button_clear = FindViewById<Button>(Resource.Id.button_clear);
            button_done = FindViewById<Button>(Resource.Id.button_done);
            plate45_button = FindViewById<ImageButton>(Resource.Id.plate45);
            plate35_button = FindViewById<ImageButton>(Resource.Id.plate35);
            plate25_button = FindViewById<ImageButton>(Resource.Id.plate25);
            plate10_button = FindViewById<ImageButton>(Resource.Id.plate10);
            plate5_button = FindViewById<ImageButton>(Resource.Id.plate5);
            plate2_5_button = FindViewById<ImageButton>(Resource.Id.plate2_5);
            barbell_button = FindViewById<ImageButton>(Resource.Id.barbell);
            ez_button = FindViewById<ImageButton>(Resource.Id.ez);
            button_delete = FindViewById<ImageButton>(Resource.Id.button_delete);
            hide_keyboard = FindViewById<ImageButton>(Resource.Id.keyboard_hide);

            // set button click listeners

            mButton1.Click += delegate
            {
                this.addNum(1);
            };
            mButton2.Click += delegate
            {
                this.addNum(2);
            };
            mButton3.Click += delegate
            {
                this.addNum(3);
            };
            mButton4.Click += delegate
            {
                this.addNum(4);
            };
            mButton5.Click += delegate
            {
                this.addNum(5);
            };
            mButton6.Click += delegate
            {
                this.addNum(6);
            };
            mButton7.Click += delegate
            {
                this.addNum(7);
            };
            mButton8.Click += delegate
            {
                this.addNum(8);
            };
            mButton9.Click += delegate
            {
                this.addNum(9);
            };
            mButton0.Click += delegate
            {
                this.addNum(0);
            };
            button_delete.Click += delegate
            {
                this.removeLast();
            };

            button_clear.Click += delegate
            {
                this.OnClick("clear");
            };
            button_done.Click += delegate
            {
                this.OnClick("done");
            };
            plate45_button.Click += delegate
            {
                this.OnClick("plate45");
            };
            plate35_button.Click += delegate
            {
                this.OnClick("plate35");
            };
            plate25_button.Click += delegate
            {
                this.OnClick("plate25");
            };
            plate10_button.Click += delegate
            {
                this.OnClick("plate10");
            };
            plate5_button.Click += delegate
            {
                this.OnClick("plate5");
            };
            plate2_5_button.Click += delegate
            {
                this.OnClick("plate2_5");
            };
            barbell_button.Click += delegate
            {
                this.OnClick("barbell");
            };
            ez_button.Click += delegate
            {
                this.OnClick("ez");
            };
            hide_keyboard.Click += delegate
            {
                keyboard.Visibility = ViewStates.Gone;
                editText.SetBackgroundResource(Resource.Drawable.input_box_background);
                this.clear();
            };

            // map buttons IDs to input strings
            keyValues.Put(Resource.Id.button_1, "1");
            keyValues.Put(Resource.Id.button_2, "2");
            keyValues.Put(Resource.Id.button_3, "3");
            keyValues.Put(Resource.Id.button_4, "4");
            keyValues.Put(Resource.Id.button_5, "5");
            keyValues.Put(Resource.Id.button_6, "6");
            keyValues.Put(Resource.Id.button_7, "7");
            keyValues.Put(Resource.Id.button_8, "8");
            keyValues.Put(Resource.Id.button_9, "9");
            keyValues.Put(Resource.Id.button_0, "0");
        }

        private void addNum(int num)
        {
            String text = editText.Text;
            if(text.Length < 3)
            {
                text = text + num.ToString();
            }
            editText.Text = text;
        }

        private void removeLast()
        {
            String text = editText.Text;
            if (text.Length > 0)
            {
                text = text.Remove(text.Length - 1, 1);
                editText.Text = text;
            }
        }

        private void clear()
        {
            plate45 = 0;
            plate35 = 0;
            plate25 = 0;
            plate10 = 0;
            plate5 = 0;
            plate2_5 = 0;
            barbell = 0;
            ez = 0;

            plate45_button.SetImageResource(Resource.Drawable.plate45);
            plate35_button.SetImageResource(Resource.Drawable.plate35);
            plate25_button.SetImageResource(Resource.Drawable.plate25);
            plate10_button.SetImageResource(Resource.Drawable.plate10);
            plate5_button.SetImageResource(Resource.Drawable.plate5);
            plate2_5_button.SetImageResource(Resource.Drawable.plate2_5);
            barbell_button.SetImageResource(Resource.Drawable.barbell);
            ez_button.SetImageResource(Resource.Drawable.ez);
        }

       
        public void OnClick(String id)
        {

            // do nothing if the InputConnection has not been set yet
            //if (inputConnection == null) return;

            if (id.Equals("clear"))
            {
                editText.Text = 0.ToString();
                clear();

            }
            else if (id.Equals("done"))
            {
                double res = plate45 * 45 + plate35 * 35 + plate25 * 25 + plate10 * 10 + plate5 * 5 + plate2_5 * 2.5 + barbell * 45 + ez * 25;
                editText.Text = (res.ToString());
                this.clear();
                editText.SetBackgroundResource(Resource.Drawable.input_box_background);
                keyboard.Visibility = ViewStates.Gone;
            }
            //else if (v.Id == Resource.Id.button_delete)
            //{
            //    CharSequence selectedText = inputConnection.getSelectedText(0);
            //    if (TextUtils.isEmpty(selectedText))
            //    {
            //        // no selection, so delete previous character
            //        inputConnection.deleteSurroundingText(1, 0);
            //    }
            //    else
            //    {
            //        // delete the selection
            //        inputConnection.commitText("", 1);
            //    }

            //}
            else if (id.Equals("plate45"))
            {
                switch (plate45)
                {
                    case 0: plate45_button.SetImageResource(Resource.Drawable.plate45_1); plate45++; break;
                    case 1: plate45_button.SetImageResource(Resource.Drawable.plate45_2); plate45++; break;
                    case 2: plate45_button.SetImageResource(Resource.Drawable.plate45_3); plate45++; break;
                    case 3: plate45_button.SetImageResource(Resource.Drawable.plate45_4); plate45++; break;
                    case 4: plate45_button.SetImageResource(Resource.Drawable.plate45_5); plate45++; break;
                    case 5: plate45_button.SetImageResource(Resource.Drawable.plate45_6); plate45++; break;
                    case 6: plate45_button.SetImageResource(Resource.Drawable.plate45_7); plate45++; break;
                    case 7: plate45_button.SetImageResource(Resource.Drawable.plate45_8); plate45++; break;
                    case 8: plate45_button.SetImageResource(Resource.Drawable.plate45_9); plate45++; break;
                    case 9: plate45_button.SetImageResource(Resource.Drawable.plate45_10); plate45++; break;
                    default: plate45_button.SetImageResource(Resource.Drawable.plate45); plate45 = 0; break;
                }
            }
            else if (id.Equals("plate35"))
            {

                switch (plate35)
                {
                    case 0: plate35_button.SetImageResource(Resource.Drawable.plate35_1); plate35++; break;
                    case 1: plate35_button.SetImageResource(Resource.Drawable.plate35_2); plate35++; break;
                    case 2: plate35_button.SetImageResource(Resource.Drawable.plate35_3); plate35++; break;
                    case 3: plate35_button.SetImageResource(Resource.Drawable.plate35_4); plate35++; break;
                    case 4: plate35_button.SetImageResource(Resource.Drawable.plate35_5); plate35++; break;
                    case 5: plate35_button.SetImageResource(Resource.Drawable.plate35_6); plate35++; break;
                    case 6: plate35_button.SetImageResource(Resource.Drawable.plate35_7); plate35++; break;
                    case 7: plate35_button.SetImageResource(Resource.Drawable.plate35_8); plate35++; break;
                    case 8: plate35_button.SetImageResource(Resource.Drawable.plate35_9); plate35++; break;
                    case 9: plate35_button.SetImageResource(Resource.Drawable.plate35_10); plate35++; break;
                    default: plate35_button.SetImageResource(Resource.Drawable.plate35); plate35 = 0; break;
                }
            }
            else if (id.Equals("plate25"))
            {
                switch (plate25)
                {
                    case 0: plate25_button.SetImageResource(Resource.Drawable.plate25_1); plate25++; break;
                    case 1: plate25_button.SetImageResource(Resource.Drawable.plate25_2); plate25++; break;
                    case 2: plate25_button.SetImageResource(Resource.Drawable.plate25_3); plate25++; break;
                    case 3: plate25_button.SetImageResource(Resource.Drawable.plate25_4); plate25++; break;
                    case 4: plate25_button.SetImageResource(Resource.Drawable.plate25_5); plate25++; break;
                    case 5: plate25_button.SetImageResource(Resource.Drawable.plate25_6); plate25++; break;
                    case 6: plate25_button.SetImageResource(Resource.Drawable.plate25_7); plate25++; break;
                    case 7: plate25_button.SetImageResource(Resource.Drawable.plate25_8); plate25++; break;
                    case 8: plate25_button.SetImageResource(Resource.Drawable.plate25_9); plate25++; break;
                    case 9: plate25_button.SetImageResource(Resource.Drawable.plate25_10); plate25++; break;
                    default: plate25_button.SetImageResource(Resource.Drawable.plate25); plate25 = 0; break;
                }
            }
            else if (id.Equals("plate10"))
            {
                switch (plate10)
                {
                    case 0: plate10_button.SetImageResource(Resource.Drawable.plate10_1); plate10++; break;
                    case 1: plate10_button.SetImageResource(Resource.Drawable.plate10_2); plate10++; break;
                    case 2: plate10_button.SetImageResource(Resource.Drawable.plate10_3); plate10++; break;
                    case 3: plate10_button.SetImageResource(Resource.Drawable.plate10_4); plate10++; break;
                    case 4: plate10_button.SetImageResource(Resource.Drawable.plate10_5); plate10++; break;
                    case 5: plate10_button.SetImageResource(Resource.Drawable.plate10_6); plate10++; break;
                    case 6: plate10_button.SetImageResource(Resource.Drawable.plate10_7); plate10++; break;
                    case 7: plate10_button.SetImageResource(Resource.Drawable.plate10_8); plate10++; break;
                    case 8: plate10_button.SetImageResource(Resource.Drawable.plate10_9); plate10++; break;
                    case 9: plate10_button.SetImageResource(Resource.Drawable.plate10_10); plate10++; break;
                    default: plate10_button.SetImageResource(Resource.Drawable.plate10); plate10 = 0; break;
                }
            }
            else if (id.Equals("plate5"))
            {
                switch (plate5)
                {
                    case 0: plate5_button.SetImageResource(Resource.Drawable.plate5_1); plate5++; break;
                    case 1: plate5_button.SetImageResource(Resource.Drawable.plate5_2); plate5++; break;
                    case 2: plate5_button.SetImageResource(Resource.Drawable.plate5_3); plate5++; break;
                    case 3: plate5_button.SetImageResource(Resource.Drawable.plate5_4); plate5++; break;
                    case 4: plate5_button.SetImageResource(Resource.Drawable.plate5_5); plate5++; break;
                    case 5: plate5_button.SetImageResource(Resource.Drawable.plate5_6); plate5++; break;
                    case 6: plate5_button.SetImageResource(Resource.Drawable.plate5_7); plate5++; break;
                    case 7: plate5_button.SetImageResource(Resource.Drawable.plate5_8); plate5++; break;
                    case 8: plate5_button.SetImageResource(Resource.Drawable.plate5_9); plate5++; break;
                    case 9: plate5_button.SetImageResource(Resource.Drawable.plate5_10); plate5++; break;
                    default: plate5_button.SetImageResource(Resource.Drawable.plate5); plate5 = 0; break;
                }
            }
            else if (id.Equals("plate2_5"))
            {
                switch (plate2_5)
                {
                    case 0:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_1);
                        plate2_5++;
                        break;
                    case 1:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_2);
                        plate2_5++;
                        break;
                    case 2:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_3);
                        plate2_5++;
                        break;
                    case 3:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_4);
                        plate2_5++;
                        break;
                    case 4:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_5);
                        plate2_5++;
                        break;
                    case 5:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_6);
                        plate2_5++;
                        break;
                    case 6:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_7);
                        plate2_5++;
                        break;
                    case 7:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_8);
                        plate2_5++;
                        break;
                    case 8:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_9);
                        plate2_5++;
                        break;
                    case 9:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5_10);
                        plate2_5++;
                        break;
                    default:
                        plate2_5_button.SetImageResource(Resource.Drawable.plate2_5);
                        plate2_5 = 0; break;
                }
            }
            else if (id.Equals("barbell"))
            {
                if (barbell == 0)
                {
                    barbell_button.SetImageResource(Resource.Drawable.barbell_1);
                    barbell++;
                }
                else
                {
                    barbell_button.SetImageResource(Resource.Drawable.barbell);
                    barbell = 0;
                }

            }
            else if (id.Equals("ez"))
            {
                if (ez == 0)
                {
                    ez_button.SetImageResource(Resource.Drawable.ez_1);
                    ez++;
                }
                else
                {
                    ez_button.SetImageResource(Resource.Drawable.ez);
                    ez = 0;
                }

            }
            //else
            //{
            //    String value = keyValues.get(v.getId());
            //    inputConnection.commitText(value, 1);
            //}
        }

        // The activity (or some parent or controller) must give us
        // a reference to the current EditText's InputConnection
        public void setInputConnection(InputConnection ic)
        {
            this.inputConnection = ic;
        }
    }
}
