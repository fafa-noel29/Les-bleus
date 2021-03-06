﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspNet_FilRouge.Models
{
    public class Seller : User
    {
        #region Attributes
        private long idSeller;
        private string password;
        private List<Order> orders;
        private Shop shop;
        private Role role;
        #endregion

        #region properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdSeller
        {
            get => idSeller;
            set => idSeller = value;
        }

        public virtual Role Role
        {
            get { return role; }
            set { role = value; }
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public List<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        public virtual Shop Shop
        {
            get { return shop; }
            set { shop = value; }
        }
        #endregion

        #region constructors
        public Seller()
        {
            this.Orders = new List<Order>();
        }
        #endregion
    }
}