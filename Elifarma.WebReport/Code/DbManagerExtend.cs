using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text; 
using System.Linq;

namespace Elifarma.WebReport.Code
{
    public partial class DbManager
    {
        public Dictionary<string, object> outputParametersResult { get; set; }
        private IEnumerable<DataRow> enumerar(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                yield return table.Rows[i];
            }
        }

        public List<t> ExecuteQuery<t>(string spname, Dictionary<string, object> parametros = null, StatementType sptype = StatementType.SQL)
        {
            List<t> objList = new List<t>();
            System.Data.DataTable dt = this.ExecuteDataTable(spname, parametros, sptype);

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(t).GetProperties(flags);
            var targetList = this.enumerar(dt).Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<t>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public DataTable ExecuteDataTable(string Query)
        {
            DataSet objData = new DataSet();

            try
            {
                using (SqlConnection cnx = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DataBase"]))
                {
                    SqlDataAdapter cmdAdaptador;
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = Query;
                    cmd.CommandTimeout = 12000;

                    cmdAdaptador = new SqlDataAdapter(cmd);

                    cnx.Open();

                    cmdAdaptador.Fill(objData);

                    cnx.Close();
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return objData.Tables[0].Copy();
        }


        public DataTable ExecuteDataTable(string spname, List<DbSqlParameter> parametros = null, StatementType type = StatementType.SQL)
        {
            DataSet objData = new DataSet();

            try
            {
                using (SqlConnection cnx = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DataBase"]))
                {
                    SqlDataAdapter cmdAdaptador;
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cnx;
                    if (type == StatementType.STOREDPROCEDURE)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;
                    cmd.CommandText = spname;

                    if (parametros != null)
                        foreach (DbSqlParameter item in parametros)
                        {
                            if (item.direction == ParameterDirectionType.IN)
                            {
                                cmd.Parameters.AddWithValue(item.parametername, item.parametervalue);
                            }
                            else if (item.direction == ParameterDirectionType.INOUT)
                            {
                                SqlParameter sqlparam = new SqlParameter();
                                sqlparam.Value = item.parametervalue;
                                sqlparam.Direction = ParameterDirection.InputOutput;
                                sqlparam.ParameterName = item.parametername;
                                cmd.Parameters.Add(sqlparam);
                            }
                            else
                            {
                                SqlParameter sqlparam = new SqlParameter();
                                sqlparam.Value = item.parametervalue;
                                sqlparam.Direction = ParameterDirection.Output;
                                sqlparam.ParameterName = item.parametername;
                                cmd.Parameters.Add(sqlparam);
                            }
                        }

                    // Verificando el tipo de control de transacción

                    cmdAdaptador = new SqlDataAdapter(cmd);

                    cnx.Open();

                    cmdAdaptador.Fill(objData);

                    outputParametersResult = new Dictionary<string, object>();

                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        {
                            outputParametersResult.Add(p.ParameterName, p.Value);
                        }
                    }

                    cnx.Close();
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return objData.Tables[0].Copy();
        }


        public DataSet ExecuteDataSet(string spname, List<DbSqlParameter> parametros = null, StatementType type = StatementType.SQL)
        {
            DataSet objData = new DataSet();

            try
            {
                using (SqlConnection cnx = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DataBase"]))
                {
                    SqlDataAdapter cmdAdaptador;
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cnx;
                    if (type == StatementType.STOREDPROCEDURE)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;
                    cmd.CommandText = spname;

                    if (parametros != null)
                        foreach (DbSqlParameter item in parametros)
                        {
                            if (item.direction == ParameterDirectionType.IN)
                            {
                                cmd.Parameters.AddWithValue(item.parametername, item.parametervalue);
                            }
                            else if (item.direction == ParameterDirectionType.INOUT)
                            {
                                SqlParameter sqlparam = new SqlParameter();
                                sqlparam.Value = item.parametervalue;
                                sqlparam.Direction = ParameterDirection.InputOutput;
                                sqlparam.ParameterName = item.parametername;
                                cmd.Parameters.Add(sqlparam);
                            }
                            else
                            {
                                SqlParameter sqlparam = new SqlParameter();
                                sqlparam.Value = item.parametervalue;
                                sqlparam.Direction = ParameterDirection.Output;
                                sqlparam.ParameterName = item.parametername;
                                cmd.Parameters.Add(sqlparam);
                            }
                        }

                    // Verificando el tipo de control de transacción

                    cmdAdaptador = new SqlDataAdapter(cmd);

                    cnx.Open();

                    cmdAdaptador.Fill(objData);

                    outputParametersResult = new Dictionary<string, object>();

                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        {
                            outputParametersResult.Add(p.ParameterName, p.Value);
                        }
                    }

                    cnx.Close();
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return objData.Copy();
        }

        public int executeNonQuery(string spname, List<DbSqlParameter> parametros = null, StatementType type = StatementType.SQL)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection cnx = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DataBase"]))
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cnx;
                    if (type == StatementType.STOREDPROCEDURE)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;
                    cmd.CommandText = spname;

                    if (parametros != null)
                        foreach (DbSqlParameter item in parametros)
                        {
                            if (item.direction == ParameterDirectionType.IN)
                            {
                                cmd.Parameters.AddWithValue(item.parametername, item.parametervalue);
                            }
                            else if (item.direction == ParameterDirectionType.INOUT)
                            {
                                SqlParameter sqlparam = new SqlParameter();
                                sqlparam.Value = item.parametervalue;
                                sqlparam.Direction = ParameterDirection.InputOutput;
                                sqlparam.ParameterName = item.parametername;
                                cmd.Parameters.Add(sqlparam);
                            }
                            else
                            {
                                SqlParameter sqlparam = new SqlParameter();
                                sqlparam.Value = item.parametervalue;
                                sqlparam.Direction = ParameterDirection.Output;
                                sqlparam.ParameterName = item.parametername;
                                sqlparam.Size = item.size;
                                cmd.Parameters.Add(sqlparam);
                            }
                        }

                    // Verificando el tipo de control de transacción
                    cnx.Open();

                    resultado = cmd.ExecuteNonQuery();

                    outputParametersResult = new Dictionary<string, object>();

                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                        {
                            outputParametersResult.Add(p.ParameterName, p.Value);
                        }
                    }

                    cnx.Close();
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return resultado;
        }


        public DataTable ExecuteDataTable(string spname, Dictionary<string, object> parametros = null, StatementType type = StatementType.SQL)
        {
            DataSet objData = new DataSet();

            try
            {
                using (SqlConnection cnx = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DataBase"]))
                {
                    SqlDataAdapter cmdAdaptador;
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cnx;
                    if (type == StatementType.STOREDPROCEDURE)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;
                    cmd.CommandText = spname;

                    if (parametros != null)
                        foreach (KeyValuePair<string, object> item in parametros)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }

                    // Verificando el tipo de control de transacción

                    cmdAdaptador = new SqlDataAdapter(cmd);

                    cnx.Open();

                    cmdAdaptador.Fill(objData);

                    cnx.Close();
                    cmd.Dispose();
                    cmd = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return objData.Tables[0].Copy();
        }

        public int ExecuteNonQuery(string spname, Dictionary<string, object> parametros = null, StatementType type = StatementType.SQL)
        {
            int resultados = 0;

            try
            {
                using (SqlConnection cnx = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DataBase"]))
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = cnx;
                    if (type == StatementType.STOREDPROCEDURE)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;
                    cmd.CommandText = spname;

                    if (parametros != null)
                        foreach (KeyValuePair<string, object> item in parametros)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }

                    cnx.Open();
                    resultados = cmd.ExecuteNonQuery();
                    cnx.Close();
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return resultados;
        }

    }

    public enum StatementType
    {
        SQL = 1,
        STOREDPROCEDURE = 2
    }

    public enum ParameterDirectionType
    {
        IN = 0,
        INOUT = 1,
        OUT = 2
    }

    public class DbSqlParameter
    {
        public string parametername { get; set; }
        public object parametervalue { get; set; }
        public ParameterDirectionType direction { get; set; } = ParameterDirectionType.IN;
        public int size;

    }
}