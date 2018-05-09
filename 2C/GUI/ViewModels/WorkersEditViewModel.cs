using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class WorkersEditViewModel : VmAdapter<Worker>
    {
        private string _name;
        private string _post;

        public WorkersEditViewModel()
        {
            Title = "Редактирование работника";
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Post
        {
            get { return _post; }
            set { SetProperty(ref _post, value); }
        }

        public override void ChangeModel()
        {
            Model.Name = Name;
            Model.Post = Post;
        }

        public override void ResetChanges()
        {
            Post = Model.Post;
            Name = Model.Name;
        }
    }
}
