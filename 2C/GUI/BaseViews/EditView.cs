﻿using Core.Models;
using GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI.BaseViews
{
    public class EditView<TModel> : BaseView<VmAdapter<TModel>>  where TModel : BaseModel
    {

    }
}
