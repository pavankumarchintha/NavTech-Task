using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApiService
{
    public class DbProcedures
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        String Query = String.Empty;
        public DbProcedures()
        {
            con = new SqlConnection("Data Source=PAVANCH;Initial Catalog=WebApiTask;Integrated Security=True");
        }
        public bool CheckFiledExists(Fields f)
        {
            try
            {
                con.Open();
                Query = "SELECT FieldName FROM EntityDetails WHERE FieldName = @FieldName";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@FieldName", f.Field);
                var strResult = (String)cmd.ExecuteScalar();
                if (String.IsNullOrEmpty(strResult))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public void InsertEntityDetails(Fields f)
        {
            try
            {
                con.Open();
                Query = "INSERT INTO EntityDetails (EntityName , FieldName,IsRequired, MaxLength,FieldSource) VALUES ('Product', @FieldName,@IsRequired,@MaxLength,@FieldSource)";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@FieldName", f.Field);
                cmd.Parameters.AddWithValue("@IsRequired", f.IsRequired);
                cmd.Parameters.AddWithValue("@MaxLength", f.MaxLength);
                cmd.Parameters.AddWithValue("@FieldSource", f.Source);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEntityDetails(String EntityName,Fields f)
        {
            try
            {
                con.Open();
                Query = "UPDATE EntityDetails SET EntityName = @EntityName, IsRequired = @IsRequired,MaxLength =@MaxLength WHERE FieldName = @FieldName";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@EntityName", EntityName);
                cmd.Parameters.AddWithValue("@FieldName", f.Field);
                cmd.Parameters.AddWithValue("@IsRequired", f.IsRequired);
                cmd.Parameters.AddWithValue("@MaxLength", f.MaxLength);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public String Getentityconfig(Fields f)
        {
            DataTable dt = new DataTable();
            String EntityName = String.Empty;
            try
            {
                Query = "SELECT * FROM EntityDetails WHERE FieldName = @FieldName";
                cmd = new SqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@FieldName", f.Field);
                sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    EntityName = Convert.ToString(row["EntityName"]);
                    f.Field = Convert.ToString(row["FieldName"]);
                    f.IsRequired = Convert.ToBoolean(Convert.ToInt16(row["IsRequired"]));
                    f.MaxLength = Convert.ToInt16(row["MaxLength"]);
                    f.Source = Convert.ToString(row["FieldSource"]);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return EntityName;
        }
    }
}