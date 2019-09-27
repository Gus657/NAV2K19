﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDeDatos
{
    class conexion
    {
        public OdbcConnection probarConexion()
        {
            OdbcConnection conn = new OdbcConnection("Dsn=navegador");// creacion de la conexion via ODBC

            try
            {
              
                conn.Open();
            }
            catch (OdbcException ex)
            {
                Console.WriteLine("No conecto: " + ex);
            }
            return conn;
        }
       
    }
}
