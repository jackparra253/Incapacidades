// @ts-check
const eslint = require('@eslint/js');
const { defineConfig } = require('eslint/config');
const tseslint = require('typescript-eslint');
const angular = require('angular-eslint');

module.exports = defineConfig([
  {
    files: ['**/*.ts'],
    extends: [
      eslint.configs.recommended,
      tseslint.configs.recommended,
      tseslint.configs.stylistic,
      angular.configs.tsRecommended,
    ],
    processor: angular.processInlineTemplates,
    rules: {
      '@angular-eslint/directive-selector': [
        'error',
        {
          type: 'attribute',
          prefix: 'app',
          style: 'camelCase',
        },
      ],
      '@angular-eslint/component-selector': [
        'error',
        {
          type: 'element',
          prefix: 'app',
          style: 'kebab-case',
        },
      ],
    },
  },
  {
    files: ['**/*.html'],
    extends: [angular.configs.templateRecommended, angular.configs.templateAccessibility],
    rules: {},
  },
  {
    files: ['src/app/domain/**/*.ts'],
    rules: {
      'no-restricted-imports': [
        'error',
        {
          patterns: [
            {
              group: ['@angular/*', '@angular/**', 'rxjs', 'rxjs/*'],
              message:
                'El dominio no conoce el framework. Si necesitás Angular acá, la lógica va en application/, infrastructure/ o ui/.',
            },
            {
              group: ['**/application/**', '**/infrastructure/**', '**/ui/**'],
              message:
                'Las dependencias apuntan hacia adentro: el dominio no importa de las capas que lo rodean.',
            },
          ],
        },
      ],
    },
  },
  {
    files: ['src/app/infrastructure/**/*.ts'],
    rules: {
      'no-restricted-imports': [
        'error',
        {
          patterns: [
            {
              group: ['**/application/**', '**/ui/**'],
              message:
                'Un adaptador implementa lo que el dominio pide; no conoce los casos de uso ni la pantalla.',
            },
          ],
        },
      ],
    },
  },
  {
    files: ['src/app/application/**/*.ts'],
    rules: {
      'no-restricted-imports': [
        'error',
        {
          patterns: [
            {
              group: ['**/ui/**'],
              message: 'Un caso de uso no conoce la pantalla que lo muestra.',
            },
          ],
        },
      ],
    },
  },
]);
