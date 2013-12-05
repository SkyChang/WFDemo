using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace NumberGuessWorkflowActivities
{

    public sealed class ReadInt : NativeActivity<int>
    {
        // 定義字串型別的活動輸入引數
        [RequiredArgument]
        public InArgument<string> BookmarkName { get; set; }

        // 如果您的活動要傳回值，請從 CodeActivity<TResult> 衍生該值，
        // 並從 Execute 方法傳回該值。
        protected override void Execute(NativeActivityContext context)
        {
            string name = BookmarkName.Get(context);

            if (name == string.Empty)
            {
                throw new ArgumentException("BookmarkName cannot be an Empty string.",
                    "BookmarkName");
            }

            // 取得 Text 輸入引數的執行階段值
            context.CreateBookmark(name, new BookmarkCallback(OnReadComplete));
        }

        // NativeActivity derived activities that do asynchronous operations by calling 
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext 
        // must override the CanInduceIdle property and return true.
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        void OnReadComplete(NativeActivityContext context, Bookmark bookmark, object state)
        {
            this.Result.Set(context, Convert.ToInt32(state));
        }
    }
}
