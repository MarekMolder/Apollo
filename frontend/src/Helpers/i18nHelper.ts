import { i18n } from '@/i18n';
import { LocalizationService } from '@/services/LocalizationService';

const resourceFiles = ['Common', 'ActionType']; // KÕIK su resx-id
const localizationService = new LocalizationService();

export async function switchLanguage(lang: string) {
  if (!i18n.global.availableLocales.includes(lang)) {
    const translationsArray = await Promise.all(
      resourceFiles.map(resource =>
        localizationService.getTranslations(lang, resource).catch(() => ({}))
      )
    );
    const merged = Object.assign({}, ...translationsArray);
    i18n.global.setLocaleMessage(lang, merged);
  }

  i18n.global.locale.value = lang;
}
