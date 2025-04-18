# fuworktimer
[![GitHub license](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/futa-t/fuworktimer/blob/main/LICENSE)
[![GitHub Release](https://img.shields.io/github/release/futa-t/fuworktimer.svg?style=flat&color=sucess)](https://github.com/futa-t/fuworktimer/releases/latest/)

## 目次
- [機能](#機能)
- [動作環境](#動作環境)
- [ダウンロード](#ダウンロード)
- [確認してる不具合](#確認してる不具合)
- [今後実装しようと思ってるもの](#今後実装しようと思ってるもの)


## 機能
#### メイン画面
![基本アプリ画面](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/kidou.png)
1. 現在アクティブなウィンドウのプロセス名が表示されます。
1. 現在アクティブなウィンドウのアクティブ時間が表示されます。  
右クリックでメニューが開きます。

#### メニュー
各種設定などができます。  

![右クリックメニュー](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/context.png)
- フォーカス[プロセス名] / フォーカス終了  
    [フォーカスモード](#フォーカスモード)を切り替えます
    
- 色の変更[プロセス名]  
    新しいプロセスが登録される際自動で背景色が生成されます。任意の色を設定したい場合にはこの機能を使ってください。

- セッション / トータル (切り替え)  
セッションではアプリを起動してからのアクティブ時間が表示されます。  
トータルでは初回起動時からの累計アクティブ時間が表示されます。

- 統計  
統計ウィンドウを開きます。

- リセット  
セッションでのすべてのウィンドウのアクティブ時間をリセットします。トータルはリセットされません。

- タスクトレイに閉じる  
閉じるボタンを押したときアプリを終了せずタスクトレイに最小化します。
タスクトレイアイコンをクリックすると再表示されます。
タスクトレイアイコンを右クリックし閉じるを選択するとアプリを終了します。

- アプリ情報  
バージョン情報などを表示します

#### 統計ウィンドウ
登録されいているプロセスの累計アクティブ時間の一覧を確認できます。

![統計ウィンドウ](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/toukei.png)

#### フォーカスモード
特定のウィンドウで集中して作業したいときのモードです。  

プロセス名に追加で`[focus]`がつきます。  
自動切り替えがオフになります。表示が変わらないだけでアクティブ時間は加算されます。  
設定したウィンドウ以外がアクティブな間はカウントが停止し背景がグレーになります。

アクティブ時  
![フォーカスモードアクティイブ時](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/focus.png)  

非アクティブ時  
![フォーカスモード非アクティイブ時](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/focusout.png)  

#### タスクトレイ
アプリ起動後タスクトレイに常駐します。マウスホバーで現在の情報が表示されれます。  
![タスクトレイアイコンとマウスホバーによる情報表示](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/tasktray.png)  

フォーカスモード中は非アクティブ時にアイコンが変化します。  
![非アクティブ時タスクトレイアイコン](https://raw.githubusercontent.com/futa-t/fuworktimer/refs/heads/main/docs/assets/tasktrayoutfocus.png)  

## 動作環境
動作確認ができている環境は以下です。
- Windows11
- [.NET 9.0 Desktop Runtime](https://dotnet.microsoft.com/ja-jp/download/dotnet/thank-you/runtime-desktop-9.0.2-windows-x64-installer)

## ダウンロード
[リリース](https://github.com/futa-t/fuworktimer/releases/latest/)から実行ファイル(fuworktimer.exe)を取得してください。  
SmartScreenの保護がでます。不安ならソースに目を通してから実行してください。

## 確認してる不具合
- 累計時間がアプリを終了するタイミングによって日付が変わったあと引きつがれない。

## 今後実装しようと思ってるもの
- 日毎や月毎でのセッション
- 記録しないウィンドウの設定
- 累計ウィンドウでのソート
- csvなどでのエクスポート機能

## プロジェクトページ
[https://github.com/futa-t/fuworktimer](https://github.com/futa-t/fuworktimer)