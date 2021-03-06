//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.using System;
using System;

using CoreGraphics;
using Foundation;
using UIKit;

using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.iOS
{
	/// <summary>
	/// Custom text field for choosing a labor type -> uses UIPickerView underneath
	/// </summary>
	[Register("LaborTypeTextField")]
	public class LaborTypeTextField : UITextField
	{
		UIToolbar toolbar;
		UIBarButtonItem done;
		UIPickerView picker;
		LaborType laborType;

		public LaborTypeTextField (IntPtr handle)
			: base(handle)
		{
			Initialize ();
		}

		public LaborTypeTextField (CGRect frame)
			: base (frame)
		{
			Initialize ();
		}

		public LaborTypeTextField ()
		{
			Initialize ();
		}

		/// <summary>
		/// Sets up the text field, toolbar, input accessory view, etc.
		/// </summary>
		void Initialize ()
		{
			toolbar = new UIToolbar (new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width, 44));
			done = new UIBarButtonItem (UIBarButtonSystemItem.Done, (sender, e) => ResignFirstResponder ());
			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
				done
			};

			picker = new UIPickerView {
				ShowSelectionIndicator = true,
				Model = new PickerModel(this)
			};
			
			InputView = picker;
			InputAccessoryView = toolbar;
			TextAlignment = UITextAlignment.Left;

			if (Theme.IsiOS7)
				done.TintColor = UIColor.White;
		}

		/// <summary>
		/// Gets or sets the current labor type
		/// </summary>
		public LaborType LaborType {
			get {
				return laborType;
			}
			set
			{
				if (laborType != value) {
					laborType = value;
					picker.Select ((int)value, 0, false);
				}
				Text = laborType.ToUserString ();
			}
		}

		protected override void Dispose (bool disposing)
		{
			done.Dispose ();
			toolbar.Dispose ();
			picker.Dispose ();

			base.Dispose (disposing);
		}

		///<summary>
		/// Sets up the "delegate" for the UIPickerView - not sure why Apple chose "ViewModel" as their wording here
		/// </summary>			
		class PickerModel : UIPickerViewModel
		{
			readonly LaborTypeTextField textField;
			readonly LaborType[] types;

			public PickerModel (LaborTypeTextField textField)
			{
				this.textField = textField;
				types = (LaborType[])Enum.GetValues (typeof(LaborType));
			}

			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				return types.Length;
			}

			public override string GetTitle (UIPickerView picker, nint row, nint component)
			{
				return types[row].ToUserString ();
			}

			public override void Selected (UIPickerView picker, nint row, nint component)
			{
				textField.LaborType = types[row];
			}
		}
	}
}

