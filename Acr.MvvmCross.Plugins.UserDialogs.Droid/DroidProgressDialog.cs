using System;
using Android.App;
using AndroidHUD;
using Cirrious.CrossCore.Droid.Platform;


namespace Acr.MvvmCross.Plugins.UserDialogs.Droid {
    
    public class DroidProgressDialog : MvxAndroidTask, IProgressDialog {

        private readonly Activity context;


        public DroidProgressDialog(Activity topActivity) {
            this.context = topActivity;
        }


        #region IProgressDialog Members

        private string title;
        public virtual string Title { 
            get { return this.title; }
            set {
                if (this.title == value)
                    return;

                this.title = value;
                this.Refresh();
            }
        }


        private int percentComplete;
        public virtual int PercentComplete {
            get { return this.percentComplete; }
            set {
                if (this.percentComplete == value)
                    return;

                if (value > 100) {
                    this.percentComplete = 100;
                }
                else if (value < 0) {
                    this.percentComplete = 0;
                }
                else {
                    this.percentComplete = value;
                }
                this.Refresh();
            }
        }


        public virtual bool IsDeterministic { get; set; }
        public virtual bool IsShowing { get; private set; }
        


        private Action cancelAction;
        private string cancelText;
        public virtual void SetCancel(Action onCancel, string cancel) {
            this.cancelAction = onCancel;
            this.cancelText = cancel;
        }


        public virtual void Show() {
            if (this.IsShowing)
                return;

            this.IsShowing = true;
            this.Refresh();
        }


        public virtual void Hide() {
            this.IsShowing = false;
            AndHUD.Shared.Dismiss(this.context);
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose() {
            this.Hide();
        }

        #endregion

        #region Internals

        protected virtual void Refresh() {
            if (!this.IsShowing)
                return;

            var p = -1;
            var txt = this.Title;
            if (this.IsDeterministic) {
                p = this.PercentComplete;
                if (!String.IsNullOrWhiteSpace(txt)) {
                    txt += "\n";
                }
                txt += p + "%\n";
            }

            if (this.cancelAction != null) {
                txt += "\n" + this.cancelText;
            }

            this.Dispatcher.RequestMainThreadAction(() => 
                AndHUD.Shared.Show(
                    this.context, 
                    txt,
                    p, 
                    MaskType.Clear,
                    null,
                    this.OnCancelClick
                )
            );
        }


        private void OnCancelClick() {
            if (this.cancelAction == null)
                return;

            this.Hide();
            this.cancelAction();
        }

        #endregion
    }
}