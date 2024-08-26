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
using GastroApi.Services;

namespace GastroApi.Models
{
    public class GastroItem
    {
        public long id { get; set; }

        public JsonRaw? data { get; set; } //public string? Data { get; set; }
    }

}

//BEFORE TRYNG THE BELOW CODE, SUBSTITUTE STRING WITH JSONRAW ON THE LAST FIELD (DATA)

//     // SPDX-FileCopyrightText: NOI Techpark <digital@noi.bz.it>
// //
// // SPDX-License-Identifier: AGPL-3.0-or-later
   

