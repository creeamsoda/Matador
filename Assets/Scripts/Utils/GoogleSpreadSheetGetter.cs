using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using UnityEditor.Localization.Plugins.Google;
using System;

namespace Utils
{
    public class GoogleSpreadSheetGetter : MonoBehaviour
    {
        public void CallGetData()
        {
            GetData();
        }
        public async UniTask GetData()
        {
            string clientId = "1011366889666-c90j6c8l5vudlt5nl77hhqcrude1s9ts.apps.googleusercontent.com";
            string clientSecret = "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";
            string user = "MatadorAuthUser";

            var secrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
            
            var scope = new[] { SheetsService.Scope.SpreadsheetsReadonly };
            
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                scope,
                user,
                System.Threading.CancellationToken.None
            );
            
            // Serviceを初期化
            var sheetService = new SheetsService(new BaseClientService.Initializer{
                HttpClientInitializer = credential
            });

            // 当該Spreadsheetのシート名一覧を出力
            var spreadSheetId = "10IQj40MaCdOns8jqRl49-kgTMMUhtu-N8DTCccVkEgs";
            var result = await sheetService.Spreadsheets.Values.Get(spreadSheetId, "ゲーム内データ!A1:L352").ExecuteAsync();
            Debug.Log(sheetService.Spreadsheets.Values.Get(spreadSheetId, "チラシ!A1:C9").Execute().Values.Count);
            //foreach (var sheet in result.Sheets)
            //{
            //    Debug.Log(sheet.Properties.Title);
            //}

            List<List<String>> translateTextList = new List<List<string>>();
            translateTextList = result.Values as List<List<string>>;

            //foreach (var sheet in result.Sheets)
            //{
            //    if (sheet.Properties.Title != "ゲーム内データ") continue;

            //    Debug.Log($"シート名: {sheet.Properties.Title}");

            //    foreach (var data in sheet.Data) { 
            //        Debug.Log(data.ToString());
            //        foreach (var row in data.RowData)
            //        {
            //            List<string> rowData = new List<string>();
            //            foreach (var cell in row.Values)
            //            {
            //                if (cell.FormattedValue != null)
            //                {
            //                    rowData.Add(cell.FormattedValue);
            //                }
            //            }
            //            translateTextList.Add(rowData);
            //        }
            //    }
            //}

            Debug.Log(result.Values.ToString());
            Debug.Log(result.Values.Count);
        }
    }
}