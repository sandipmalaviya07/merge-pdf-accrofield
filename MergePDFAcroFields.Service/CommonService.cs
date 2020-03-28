using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MergePDFAcroFields;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace MergePDFAcroFields.Service
{
    public class CommonService
    {
        #region "PDF Merge Functions"
        /// <summary>
        /// Common function for all three but the data  will set according to key value that 
        /// we're using to set 
        /// </summary>
        /// <returns></returns>
        public void SetPDFFields(dynamic _accrofield)
        {
            try
            {
                _accrofield.SetField("field1", "First PDF Label1 "); // Test 1 PDF Label
                _accrofield.SetField("field2", "Second PDF Label1"); // Test 2 PDF Label 
                _accrofield.SetField("field3", "Third PDF Label1");  // Test 3 PDF Label
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
    }
}
