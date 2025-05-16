import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'
import eslintPluginPrettier from 'eslint-plugin-prettier'
import eslintPluginReact from 'eslint-plugin-react'
import eslintPluginTailwindCSS from 'eslint-plugin-tailwindcss'
import eslintConfig from 'eslint-config-prettier'
import eslintPluginImport from 'eslint-plugin-import'
import eslintUnusedImports from 'eslint-plugin-unused-imports'
import eslintPluginReactQuery from 'eslint-plugin-react-query'

export default tseslint.config(
  { ignores: ['dist'] },
  {
    extends: [js.configs.recommended, ...tseslint.configs.recommended],
    files: ['**/*.{ts,tsx}'],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    plugins: {
      'react-hooks': reactHooks,
      'react-refresh': reactRefresh,
      'prettier' : eslintPluginPrettier,
      react: eslintPluginReact,
      'unused-imports': eslintUnusedImports,
      import: eslintPluginImport,
      prettier: eslintConfig,
      'eslint-plugin-import': eslintPluginImport,
      tailwindcss: eslintPluginTailwindCSS,
      'react-query': eslintPluginReactQuery,
    },
    rules: {
      ...reactHooks.configs.recommended.rules,
      ...eslintPluginReact.configs.recommended.rules,
      ...eslintPluginReactQuery.configs.recommended.rules,
      ...eslintPluginTailwindCSS.configs.recommended.rules,
      ...eslintPluginImport.configs.recommended.rules,
      ...eslintConfig.rules,
      ...eslintPluginPrettier.configs.recommended.rules,
      ...eslintPluginReact.configs.all.rules,
      ...eslintPluginReactQuery.configs.all.rules,
      ...eslintPluginTailwindCSS.configs.all.rules,
      ...eslintPluginImport.configs.all.rules,
      ...eslintConfig.rules,
      ...eslintPluginPrettier.configs.all.rules,     
      'react-refresh/only-export-components': [
        'warn',
        { allowConstantExport: true },
      ],
    },
  },
)
