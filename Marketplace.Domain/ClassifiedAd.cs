﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain
{
    public class ClassifiedAd
    {
        public ClassifiedAdId Id { get; }
        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            _ownerId = ownerId;
        }
        public void SetTitle(string title) => _title = title;
        public void UpdateText(string text) => _text = text;
        public void UpdatePrice(decimal price) => _price = price;
        private UserId _ownerId;
        private string _title;
        private string _text;
        private decimal _price;
    }
}
