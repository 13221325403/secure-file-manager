   M S B A M L   `   `   `  ���� G  CGestioneFile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:GestioneFileGestioneFile  &     GestioneFile.WindowGestioneFiles   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:GestioneFile     -   $��625.2��$��1202.8��$	��670��$
��1240��  � 
ResizeMode$  CanResizeWithGrip=� � Icon$ icon.ico��.��� -    �Name$ 	grid_main��$��0q� � ColumnDefinitions �� $	��250 ��� �� $	��300 �.+�o� $��20��$	��Top=�$��3��.@�m� $��Database��$��20��.@�m� -   $
��apri��m� -   $��elimina��m� $��Cartelle��.@�m� -   $��Aggiungi��m� -   $��Rinomina��m� -   $��Elimina���� m� -   $��Esporta cartella��m� $
��File��.@�m� -	   $��Aggiungi��m� -
   $��Elimina���� m� -   $��Esporta���� m� -   $��Cerca��m� $��
Protezione��.@�m� -    m�Name$ mi_imposta_password��$��Imposta password��m� -   $ mi_rimuovi_password��$��Rimuovi password���� -    ��Name$ lbxDir��$��10,41,4.6,24.8q�$��1q� � ContextMenu �� .@�m� -   $��Rinomina��m� -   $��Modifica descrizione��m� -   $��Elimina��m� -   $��Esporta cartella��&   System.Windows.Controls.DataGrid  -     Name$ dgFiles��$��	5,41,5,25q�$��1q�   CanUserAddRows .  	   CanUserResizeRows	 .  
   CanUserDeleteRows
 .     
IsReadOnly .    AutoGenerateColumns .  $��1��
����������	#FFABADB3 �� .@�m� -   $��elimina��m� -   $��esporta��   VerticalGridLinesBrush ��  �� Color# � ��   HorizontalGridLinesBrush �� # � ��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Nome��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *nome+  $ 
Estensione�� �� *ext+  $ 
Dimensioni�� �� *
dimensioni+  $ Data creazione�� �� *data_creazione+  $ Data ultima modifica�� �� *data_ultima_modifica+  $ Dettagli�� �� *descrizione+�� $��0q�$��3��$��20��$��Bottom=��� -    ��Name$ 	textBlock��$
��Left=�$��	10,25,0,0q�$
��Wrap=�$��cartelle��$	��Top=��� -   $ textBlock_Copy��$
��Left=�$��5,25,0,0q�$
��Wrap=�$��File della cartella��$	��Top=�$��1���� -   $ textBlock_Copy1��$
��Left=�$��5,25,0,0q�$
��Wrap=�$��File selezionato��$	��Top=�$��2��� -   $ grid_sel_file��$��2��$��
5,41,10,25q�
�������.+�� $��32��$��Bottom=� �� $	��17* ��� $	��17* ��� $	��17* ��� $	��17* �.+��� -    ��Name$ btn_salva_modifiche��$��Salva��$��5q��� -   $��Elimina��$��5q�$��1���� -   $��Esporta��$��5q�$��2���� -   $
��Apri��$��5q�$��3��� 
�������$��0,285,0,36.6q�.+��� -    ��Name$ label��$��Nome:��$
��Left=�$��5,5,0,0q�$	��Top=��� -    $ 
label_Copy��$��Dimensioni:��$
��Left=�$��5,57,0,0q�$	��Top=��� -!   $ label_Copy1��$��Data creazione:��$
��Left=�$��5,83,0,0q�$	��Top=��� -"   $ label_Copy2��$��	Dettagli:��$
��Left=�$��	5,161,0,0q�$	��Top=��� -#    ��Name$ txt_data_creazione��$��20��$��
106,87,5,0q� �� TextWrapping$
 Wrap=�$	��Top=�{�.  ���	#FFABADB3��Z� �� -$   $ txt_dimensioni��$��20��$��
106,61,0,0q�$
 Wrap=�$	��Top=�{�.  $
��Left=�$	��104�����	#FFABADB3��Z� �� -%   $ txt_nome��$��20��$��	106,9,5,0q�$	��Top=� �� 	MaxLength$	 100���� -&   $ txt_dettagli��$��106,165,5,0q�$	��Top=�$	 200��$��20���� -'   $ label_Copy3��$��Ultima modifica:��$
��Left=�$��	5,109,0,0q�$	��Top=��� -(   $ txt_data_ultima_modifica��$��20��$��106,113,5,0q�$
 Wrap=�$	��Top=�{�.  ���	#FFABADB3��Z� �� -)    ��Name$ cmb_fk_cartella��$��106,140,5,0q�$	��Top=�$��20��$��6,0,5,0q�$��Center=��� -*   $ label_Copy4��$��	Cartella:��$
��Left=�$��	5,135,0,0q�$	��Top=��� -+   $ label_Copy5��$��Estensione:��$
��Left=�$��5,31,0,0q�$	��Top=��� -,   $ txt_ext��$��20��$��
106,35,0,0q�$
 Wrap=�$	��Top=�{�.  $
��Left=�$	��104�����	#FFABADB3��Z� � --   $ grid_preview��$	��285��$	��Top=����� # � ��