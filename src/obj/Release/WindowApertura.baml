   M S B A M L   `   `   `  ���� G  CGestioneFile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:GestioneFileGestioneFile  !    GestioneFile.WindowApertura   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:GestioneFile     -   $	��186��$	��488��  � 
ResizeMode$  NoResize=� � Icon$ icon.ico��.��� $��0q�.+��� $��File database:��$
��Left=�$��	10,42,0,0q�$	��Top=��� -    ��Name$ txt_db_path��$
��Left=�$��20��$��
130,47,0,0q�$	��Top=�$	��275��$��Center=��� -    ��Name$ 
btn_carica��$	��...��$
��Left=�$��
410,47,0,0q�$	��Top=�$��30���� $��
Modalità:��$
��Left=�$��	10,10,0,0q�$	��Top=�	� -    	�Name$ rdb_crea��$��Crea nuovo file��$
��Left=�$��
255,16,0,0q�$	��Top=�$��Center=� � ToolTip$1 +crea un nuovo file specificando il percorso��	� -   $ rdb_apri�� d� 	IsChecked$
 TrueY�$��Apri file esistente��$
��Left=�$��
130,16,0,0q�$	��Top=�$��Center=�$3 -Apri un file esistente precedentemente creato���� -   $ 
btn_accedi��$
��Apri��$
��Left=�$��	10,0,0,10q�$��Bottom=�$��82��{�.   �� 	IsDefault . �� -    ��Name$ 	lbl_error��$�� ��$
��Left=�$��	130,0,0,7q�$��Bottom=����	#FFC10000�� $��	Password:��$
��Left=�$��	10,73,0,0q�$	��Top=�G� -   	 G�Name$	 pwb_password��$
��Left=�$��
130,76,0,0q�$	��Top=�$	��275��$��Center=�$��20��