# MakeAtomCraftData
CRI Atom Craftのデータを自動生成する
- Unity5上のC#スクリプトでADX2のワークユニットの生成をします。
- Assets/Materialsにあるwavファイルを元にキューが作られます。
- Assetsフォルダ以下に指定した名前のワークユニットが含まれたフォルダが生成されます。

#セットアップ
* Assets下にEditorフォルダを作成します。
* EditorフォルダにMakeAtomCraftData.cs、MakeAtomCraftACF.csをコピーします。

#使い方1.(ワークユニットファイル(ACB)の生成）
* Assets下にMaterialsフォルダを用意します。
* 使いたい波形を登録します
* メニュー「CRI>My>Open MakeAtomCraft...」を選択します。
* キューシート名を設定して「Make Atom Craft Data」ボタンを押します。

#使い方2.（全体設定(ACF)の生成）
* メニュー「CRI>My>Open MakeAtomACF...」を選択します。
* ACF名、カテゴリを設定して「Make Atom ACF Data」ボタンを押します。

#使い方3.（AtomCraftへの読み込み）
できあがったatmcglobalsやwrokunitフォルダを、新規作成したAtomCraftのファイルへ上書きします。

動画→https://www.youtube.com/watch?v=vCJa73xqK6I
（仕様が変わったのでワークユニットに登録するところのみ参考に）

# 注意
- Assetsフォルダにキューシート名のフォルダが既にある時はあらかじめ削除してから開始してください。
- グループ／カテゴリの設定は全体設定側で正しく行った状態で読み込まないと読み込み時リンクエラーが発生します。この場合はグループ／カテゴリを正しく作成しなおしてください。
- CRI Atom CraftはVer.2.14.00、Unityは5.1.0f3で動作確認しています。
