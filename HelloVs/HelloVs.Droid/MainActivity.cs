using System;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Widget;
using Android.OS;
using Android.Provider;

namespace HelloVs.Droid
{
	[Activity (Label = "HelloVs.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
	    public const int ResultLoadImage = 1;
        private ImageView _imageView;

		protected override void OnCreate (Bundle bundle)
		{
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += ButtonOnClick;
		}

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), ResultLoadImage);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == ResultLoadImage) && (resultCode == Result.Ok) && (data != null))
            {
                var uri = data.Data;
                _imageView.SetImageURI(uri);

                var path = GetPathToImage(uri);
                Toast.MakeText(this, path, ToastLength.Long);
            }
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string path;
            var projection = new[] { MediaStore.Images.Media.InterfaceConsts.Data };
            using (var cursor = ContentResolver.Query(uri, projection, null, null, null))
            {
                if (cursor == null) return null;
                var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }
            return path;
        }
	}
}


