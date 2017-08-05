﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JanuszMarcinik.Mvc.DataSource
{
    public abstract class DataSource<TModel> : IGrid where TModel : class
    {
        public IEnumerable<TModel> Data { get; set; }

        public List<GridHeader> Headers { get; set; }
        public List<GridRow> Rows { get; set; }

        public int PageIndex { get; set; }
        public PageSize PageSize { get; set; }
        public int TotalRows { get; set; }
        public string PagerResult
        {
            get
            {
                var startIndex = this.PageIndex * (int)this.PageSize - (int)this.PageSize + 1;
                var endIndex = startIndex + (int)this.PageSize - 1;

                if (startIndex >= this.TotalRows)
                {
                    startIndex = this.TotalRows;
                }

                if (endIndex >= this.TotalRows)
                {
                    endIndex = this.TotalRows;
                }

                return $"Wyniki: {startIndex}-{endIndex} z {this.TotalRows}";
            }
        }

        public string LastOrderBy { get; set; }
        public string OrderBy { get; set; }
        public SortOrder SortOrder { get; set; }

        #region Initialize()
        public virtual void Initialize()
        {
            if (this.Data == null)
            {
                this.Data = new List<TModel>();
            }

            Filter();

            this.Headers = new List<GridHeader>();
            this.TotalRows = this.Data.Count();
            if (this.TotalRows > 0)
            {
                SetHeaders(this.Data.First().GetType().GetProperties());
            }

            Pager();
            Sort();
            SetRows();
            SetEditActions();
        }
        #endregion

        #region SetHeaders()
        private void SetHeaders(PropertyInfo[] properties)
        {
            this.Headers = new List<GridHeader>();

            foreach (var prop in properties)
            {
                var header = new GridHeader()
                {
                    DisplayName = prop.Name,
                    PropertyName = prop.Name
                };

                if (prop.Name == "Id")
                {
                    header.IsPrimaryKey = true;
                    this.Headers.Add(header);
                }
                else
                {
                    var gridAttribute = prop.GetCustomAttribute<GridAttribute>();
                    if (gridAttribute != null)
                    {
                        var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
                        if (displayAttribute != null)
                        {
                            header.DisplayName = displayAttribute.Name;
                        }

                        header.Order = gridAttribute.Order;

                        if (gridAttribute.IsImagePath)
                        {
                            header.IsImagePath = true;
                            header.DisplayName = string.Empty;
                        }

                        this.Headers.Add(header);
                    }
                }
            }

            if (properties.Count() > 1)
            {
                this.Headers = this.Headers.OrderBy(x => x.Order).ToList();
            }
        }
        #endregion

        #region SetRows()
        private void SetRows()
        {
            this.Rows = new List<GridRow>();

            foreach (var item in this.Data)
            {
                var row = new GridRow();
                foreach (var prop in this.Headers)
                {
                    try
                    {
                        if (prop.IsPrimaryKey)
                        {
                            row.PrimaryKeyId = (int)item.GetType().GetProperty(prop.PropertyName).GetValue(item);
                        }
                        else if (prop.IsImagePath)
                        {
                            row.ImagePath = item.GetType().GetProperty(prop.PropertyName).GetValue(item).ToString();
                        }
                        else if (item.GetType().GetProperty(prop.PropertyName).GetValue(item).GetType().BaseType == typeof(Enum))
                        {
                            var enumValue = (Enum)item.GetType().GetProperty(prop.PropertyName).GetValue(item);
                            row.Values.Add(enumValue.GetDescription());
                        }
                        else
                        {
                            row.Values.Add(item.GetType().GetProperty(prop.PropertyName).GetValue(item).ToString());
                        }
                    }
                    catch
                    {
                        row.Values.Add(string.Empty);
                    }
                }

                this.Rows.Add(row);
            }
        }
        #endregion

        #region Pager()
        private void Pager()
        {
            if (this.PageSize == PageSize.Unset)
            {
                this.PageSize = PageSize.Ten;
            }
            if (this.PageIndex == 0)
            {
                this.PageIndex = 1;
            }

            this.Data = this.Data
                .Skip((this.PageIndex * (int)this.PageSize) - (int)this.PageSize)
                .Take((int)this.PageSize);
        }
        #endregion

        #region Sort()
        private void Sort()
        {
            if (!string.IsNullOrEmpty(this.OrderBy))
            {
                if (this.SortOrder == SortOrder.Unset)
                {
                    this.SortOrder = SortOrder.Ascending;
                }
                else if (this.SortOrder == SortOrder.Ascending && this.LastOrderBy == this.OrderBy)
                {
                    this.SortOrder = SortOrder.Descending;
                }
                else if (this.SortOrder == SortOrder.Descending)
                {
                    this.SortOrder = SortOrder.Ascending;
                }

                if (this.SortOrder == SortOrder.Ascending)
                {
                    this.Data = this.Data.OrderBy(x => x.GetType().GetProperty(this.OrderBy).GetValue(x, null));
                }
                else if (this.SortOrder == SortOrder.Descending)
                {
                    this.Data = this.Data.OrderByDescending(x => x.GetType().GetProperty(this.OrderBy).GetValue(x, null));
                }
            }

            this.LastOrderBy = this.OrderBy;
        }
        #endregion

        protected abstract void SetEditActions();

        protected abstract void Filter();
    }
}