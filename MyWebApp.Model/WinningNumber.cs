using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApp.Model
{
    public class WinningNumber
    {
		public string dataFrom { get; set; }
		public string emptyFlag { get; set; }
		public string errorCode { get; set; }
		public string errorMessage { get; set; }
		public bool success { get; set; }
		public Value value { get; set; }
	}


	public class List
	{
		public string drawFlowFund { get; set; }
		public string drawFlowFundRj { get; set; }
		public string estimateDrawTime { get; set; }
		public string isGetKjpdf { get; set; }
		public string isGetXlpdf { get; set; }
		public string lotteryDrawNum { get; set; }
		public string lotteryDrawResult { get; set; }
		
	}

	public class Value
	{
		public List<List> list { get; set; }
		public string pageNo { get; set; }
		public string pageSize { get; set; }
		public string pages { get; set; }
		public string total { get; set; }
	}
}
