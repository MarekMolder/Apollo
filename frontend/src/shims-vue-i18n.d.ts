import 'vue-i18n';

declare module 'vue' {
  interface ComponentCustomProperties {
    $t: (key: string) => string;
    $tc: (key: string, choice?: number, ...args: unknown[]) => string;
  }
}
