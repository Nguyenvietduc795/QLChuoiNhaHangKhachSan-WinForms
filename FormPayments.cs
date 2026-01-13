// change the command text to include schema
using (SqlCommand cmd = new SqlCommand("dbo.sp_GetTransactionHistory", conn) { CommandType = CommandType.StoredProcedure })
{
    // ...
}

catch (Exception ex)
{
    // Show full exception (stack + inner) while debugging
    MessageBox.Show("L?i: " + ex.ToString());
}