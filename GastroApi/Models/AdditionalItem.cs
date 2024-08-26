// SPDX-FileCopyrightText: NOI Techpark <digital@noi.bz.it>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static Dapper.SqlMapper;

namespace GastroApi.Models

{

 public class AdditionalItem
    {

        public string? DescriptionName { get; set; }

        public string? Ingredients { get; set; }

        public string? Recipe { get; set; }

        public int? TimeToPrepare { get; set; }
    } 

   




}